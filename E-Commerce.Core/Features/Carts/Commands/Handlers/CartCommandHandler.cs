using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Features.Carts.Commands.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Carts.Commands.Handlers
{
    public class CartCommandHandler : ApiResponseHandler,
        IRequestHandler<EditCartCommand, ApiResponse<EditCartResponse>>,
        IRequestHandler<DeleteCartCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        #endregion

        #region Constructors
        public CartCommandHandler(ICartService cartService,
            ICurrentUserService currentUserService,
            IProductService productService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cartService = cartService;
            _currentUserService = currentUserService;
            _productService = productService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<EditCartResponse>> Handle(EditCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var existingCart = await _cartService.GetCartByIdAsync(request.Cart.Id);
            var cartMapper = new Cart
            {
                Id = request.Cart.Id,
                CreatedAt = existingCart?.CreatedAt ?? DateTimeOffset.UtcNow.ToLocalTime(),
                CustomerId = userId,
                CartItems = request.Cart.CartItems!.Select(item =>
                {
                    var existingItem = existingCart?.CartItems?.FirstOrDefault(x => x.ProductId == item.ProductId);
                    return new CartItem
                    {
                        CartId = request.Cart.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        CreatedAt = existingItem?.CreatedAt ?? DateTimeOffset.UtcNow.ToLocalTime()
                    };
                }).ToList()
            };

            var cart = await _cartService.UpdateCartAsync(cartMapper);
            if (cart is null) return BadRequest<EditCartResponse>();

            var productIds = cart.CartItems.Select(i => i.ProductId).ToList();
            var productsDict = await _productService.GetProductsByIdsAsync(productIds);

            return Success(new EditCartResponse
            {
                Id = cart.Id,
                CreatedAt = cart.CreatedAt,
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems?.Select(item => new CartItemResponse
                {
                    ProductId = item.ProductId,
                    ProductName = productsDict.TryGetValue(item.ProductId, out var name) ? name : null,
                    Quantity = item.Quantity,
                    CreatedAt = item.CreatedAt
                }).ToList()
            });
        }

        public async Task<ApiResponse<string>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.DeleteCartAsync(request.CartId);
            if (result) return Deleted<string>();
            return BadRequest<string>();
        }
        #endregion
    }
}
