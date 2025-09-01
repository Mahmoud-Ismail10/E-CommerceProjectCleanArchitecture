using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Core.Features.Orders.Commands.Responses;
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
        IRequestHandler<AddOrderCommand, Guid>,
        IRequestHandler<PlaceOrderCommand, ApiResponse<PaymentProcessResponse>>,
        IRequestHandler<DeleteOrderCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public OrderCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IOrderService orderService,
            IProductService productService,
            ICartService cartService,
            ICurrentUserService currentUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _orderService = orderService;
            _productService = productService;
            _cartService = cartService;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<Guid> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            // Check if the user is authenticated
            if (!_currentUserService.IsAuthenticated)
                throw new InvalidOperationException(_stringLocalizer[SharedResourcesKeys.PleaseLoginFirst]);

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
            if (cart == null || cart.CartItems.Count == 0)
                throw new InvalidOperationException(_stringLocalizer[SharedResourcesKeys.CartNotFoundOrEmpty]);

            var order = new Order();

            // Validate and process each cart item
            foreach (var item in cart.CartItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID {item.ProductId} does not exist.");

                if (product.Price == null || product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"Product {product.Name} is not available or stock is insufficient.");

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    SubAmount = item.Quantity * (product.Price ?? 0)
                });
            }

            order.CustomerId = cart.CustomerId;
            order.OrderDate = DateTimeOffset.UtcNow.ToLocalTime();
            order.TotalAmount = order.OrderItems.Sum(i => i.SubAmount);
            order.Status = Status.Draft;

            // Add Order and return result
            var result2 = await _orderService.AddOrderAsync(order);
            if (result2 != "Success")
                throw new InvalidOperationException(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
            return order.Id;
        }


        public async Task<ApiResponse<PaymentProcessResponse>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null) return NotFound<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.OrderNotFound]);
            var result = await _orderService.PlaceOrderAsync(order);
            return result switch
            {
                "CartIsEmpty" => NotFound<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.CartNotFoundOrEmpty]),
                "ProductNotFound" => NotFound<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]),
                "ErrorInPlacedOrder" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.ErrorInPlacedOrder]),
                "NoCustomerFoundForOrder" => NotFound<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.NoCustomerFoundForOrder]),
                "FailedToConfirmCODOrder" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.FailedToConfirmCODOrder]),
                "PaymentMethodNotSelected" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.PaymentMethodNotSelected]),
                "PaymobIframeIDIsNotConfigured" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.PaymobIframeIDIsNotConfigured]),
                "FailedToProcessPaymentForOrder" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.FailedToProcessPaymentForOrder]),
                "PaymentTokenCannotBeNullOrEmpty" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.PaymentTokenCannotBeNullOrEmpty]),
                "FailedToPersistOnlinePaymentData" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.FailedToPersistOnlinePaymentData]),
                "FailedInDiscountQuantityFromStock" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.FailedInDiscountQuantityFromStock]),
                "ShippingAddressIsRequiredForHomeDelivery" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.ShippingAddressIsRequiredForHomeDelivery]),
                "InvalidPaymobIntegrationIDInConfiguration" => BadRequest<PaymentProcessResponse>(_stringLocalizer[SharedResourcesKeys.InvalidPaymobIntegrationIDInConfiguration]),
                _ => Success(new PaymentProcessResponse(order.Id, result, order.PaymentToken!))
            };
        }

        public async Task<ApiResponse<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.OrderNotFound]);
            var result = await _orderService.DeleteOrderAsync(order);
            return result != "Success" ? BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]) : Deleted<string>();
        }
        #endregion
    }
}
