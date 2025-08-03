using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Orders.Commands.Handlers
{
    public class OrderCommandHandler : ApiResponseHandler,
        IRequestHandler<AddOrderCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly IDeliveryService _deliveryService;
        private readonly IShippingAddressService _shippingAddressService;
        #endregion

        #region Constructors
        public OrderCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IMapper mapper,
            IOrderService orderService,
            IProductService productService,
            IPaymentService paymentService,
            IDeliveryService deliveryService,
            IShippingAddressService shippingAddressService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _orderService = orderService;
            _productService = productService;
            _paymentService = paymentService;
            _deliveryService = deliveryService;
            _shippingAddressService = shippingAddressService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in request.OrderItemResults!)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    return BadRequest<string>($"Product with ID {item.ProductId} does not exist.");

                if (product.Price == null || product.StockQuantity < item.Quantity)
                    return BadRequest<string>($"Product {product.Name} is not available or stock is insufficient.");

                totalAmount += product.Price.Value * item.Quantity;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            // Payment Settings
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                PaymentMethod = request.PaymentMethod,
                Amount = totalAmount,
                Status = Status.Pending
            };

            // Delivery Settings
            Delivery? delivery = null;
            if (request.DeliveryMethod != DeliveryMethod.Pickup)
            {
                var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync((Guid)request.ShippingAddressId!);
                if (shippingAddress == null)
                    return BadRequest<string>($"Shipping Address with ID {request.ShippingAddressId} does not exist.");

                var deliveryOffset = DeliveryTimeCalculator.Calculate(shippingAddress.City, request.DeliveryMethod);
                var deliveryCost = DeliveryCostCalculator.Calculate(shippingAddress.City, request.DeliveryMethod);

                delivery = new Delivery
                {
                    Id = Guid.NewGuid(),
                    DeliveryMethod = request.DeliveryMethod,
                    Description = $"Delivery for order {order.Id}",
                    DeliveryTime = DateTime.UtcNow.Add(deliveryOffset),
                    Cost = deliveryCost
                };
            }

            var result = await _orderService.AddOrderAsync(order, orderItems, payment, delivery);
            return result switch
            {
                "Success" => Created(""),
                "FailedInDiscountQuantityFromStock" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInDiscountQuantityFromStock]),
                "FailedInPaymentProcessing" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInPaymentProcessing]),
                "FailedInDeliveryProcessing" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInDeliveryProcessing]),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed])
            };
        }
        #endregion
    }
}
