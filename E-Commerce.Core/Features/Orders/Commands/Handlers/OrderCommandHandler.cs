using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Service.AuthService.Services.Contract;
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
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public OrderCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IMapper mapper,
            IOrderService orderService,
            IProductService productService,
            ICartService cartService,
            ICurrentUserService currentUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _orderService = orderService;
            _productService = productService;
            _cartService = cartService;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            // Check if the user is authenticated
            if (!_currentUserService.IsAuthenticated)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.PleaseLoginFirst]);

            // Retrieve the cart
            var userId = _currentUserService.GetCartOwnerId();
            var cartKey = $"cart:{userId}";

            var result1 = await _cartService.MigrateGuestCartToCustomerAsync(userId);
            var badRequestMessage = result1 switch
            {
                "FailedInEditCart" => _stringLocalizer[SharedResourcesKeys.FailedToModifyThisCart],
                "TransactionFailedToCommit" => _stringLocalizer[SharedResourcesKeys.TransactionFailedToCommit],
                "FailedInMigrateGuestCartToCustomer" => _stringLocalizer[SharedResourcesKeys.FailedInMigrateGuestCartToCustomer],
                "FailedToDeleteGuestIdCookie" => _stringLocalizer[SharedResourcesKeys.FailedToDeleteGuestIdCookie],
                _ => null
            };

            if (badRequestMessage != null)
                BadRequest<string>(badRequestMessage);

            var cart = await _cartService.GetCartByKeyAsync(cartKey);
            if (cart == null) return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CartNotFound]);

            //var order = _mapper.Map<Order>(request);
            var order = new Order();
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
                    SubAmount = item.Quantity * (product.Price ?? 0)
                });
            }

            // Convert cart to order
            order.Payment ??= new Payment();
            order.Delivery ??= new Delivery();
            //order.ShippingAddress ??= new ShippingAddress();

            order.CustomerId = cart.CustomerId;
            order.OrderDate = DateTimeOffset.UtcNow.ToLocalTime();
            order.Status = Status.Pending;
            order.TotalAmount = orderItems.Sum(i => i.SubAmount);
            order.OrderItems = orderItems;

            order.Payment.Id = Guid.NewGuid();
            order.Payment.OrderId = order.Id;
            order.Payment.PaymentMethod = request.PaymentMethod;
            order.Payment.Status = Status.Pending;
            order.Payment.TotalAmount = order.TotalAmount;
            order.Payment.PaymentDate = DateTimeOffset.UtcNow.ToLocalTime();
            order.Payment.TransactionId = Guid.NewGuid().ToString();
            order.PaymentId = order.Payment.Id;
            order.PaymentToken = Guid.NewGuid().ToString();

            order.Delivery.DeliveryMethod = request.DeliveryMethod;
            if (request.ShippingAddressId is not null)
            {
                order.ShippingAddressId = request.ShippingAddressId;
                //order.ShippingAddress!.Id = (Guid)request.ShippingAddressId;
            }

            // Add Order and return result
            var result2 = await _orderService.AddOrderAsync(order);
            return result2 switch
            {
                "FailedInAdd" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]),
                "FailedInDeletingCart" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInDeletingCart]),
                "FailedInPaymentProcessing" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInPaymentProcessing]),
                "FailedInDeliveryProcessing" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInDeliveryProcessing]),
                "ShippingAddressDoesNotExist" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.ShippingAddressDoesNotExist]),
                "FailedInDiscountQuantityFromStock" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedInDiscountQuantityFromStock]),
                _ => Created("")
            };
        }
        #endregion
    }
}
