using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Carts.Queries.Models;
using E_Commerce.Core.Features.Carts.Queries.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Carts.Queries.Handlers
{
    public class CartQueryHandler : ApiResponseHandler, IRequestHandler<GetCartByIdQuery, ApiResponse<GetSingleCartResponse>>
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        #endregion

        #region Constructors
        public CartQueryHandler(ICartService cartService,
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
        public async Task<ApiResponse<GetSingleCartResponse>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Get cart from Redis
            var cart = await _cartService.GetCartByIdAsync(request.Id);
            if (cart == null) return NotFound<GetSingleCartResponse>();

            // 2. Extract ProductIds
            var productIds = cart.CartItems.Select(i => i.ProductId).ToList();

            // 3. Query DB to get product names
            var products = await _productService.GetProductsByIdsAsync(productIds);

            // 4. Map response
            var cartMapper = new GetSingleCartResponse
            {
                Id = cart.Id,
                CreatedAt = cart.CreatedAt,
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems?.Select(item => new CartItemOfGetSingleResponse
                {
                    ProductId = item.ProductId,
                    ProductName = products.TryGetValue(item.ProductId, out var name) ? name : null,
                    Quantity = item.Quantity,
                    CreatedAt = item.CreatedAt
                }).ToList()
            };

            // 5. Return response
            var userId = _currentUserService.GetUserId();
            var resultCart = cartMapper ?? new GetSingleCartResponse { Id = request.Id, CustomerId = userId, CreatedAt = DateTime.UtcNow };
            return Success(resultCart);
        }
        #endregion
    }
}
