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
        private readonly ICartService _cartService;
        private readonly IShippingAddressService _shippingAddressService;
        #endregion

        #region Constructors
        public OrderCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IMapper mapper,
            IOrderService orderService,
            IProductService productService,
            ICartService cartService,
            IShippingAddressService shippingAddressService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _orderService = orderService;
            _productService = productService;
            _cartService = cartService;
            _shippingAddressService = shippingAddressService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the cart
            var cart = await _cartService.GetMyCartAsync();
            if (cart == null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CartNotFound]);

            var order = _mapper.Map<Order>(request);

            var orderItems = new List<OrderItem>();

            // Validate and process each cart item
            foreach (var item in cart.CartItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    return BadRequest<string>($"Product with ID {item.ProductId} does not exist.");

                if (product.Price == null || product.StockQuantity < item.Quantity)
                    return BadRequest<string>($"Product {product.Name} is not available or stock is insufficient.");

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    SubAmount = item.SubAmount
                });
            }

            // Payment Settings
            //var payment = new Payment
            //{
            //    Id = Guid.NewGuid(),
            //    PaymentMethod = request.PaymentMethod,
            //    TotalAmount = cart.TotalAmount,
            //    Status = Status.Pending
            //};

            // Convert cart to order
            order.CustomerId = cart.CustomerId;
            order.OrderDate = DateTimeOffset.UtcNow.ToLocalTime();
            order.Status = Status.Pending;
            order.TotalAmount = cart.TotalAmount;

            // Delivery Settings
            Delivery? delivery = null;
            if (request.DeliveryMethod != DeliveryMethod.PickupFromBranch)
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
                    Cost = deliveryCost,
                    Status = Status.Pending,
                };
            }

            // Add Order and return result
            var result = await _orderService.AddOrderAsync(order, orderItems, delivery);
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
