using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Carts.Commands.Handlers
{
    public class CartCommandHandler : ApiResponseHandler,
        IRequestHandler<AddToCartCommand, ApiResponse<string>>,
        IRequestHandler<RemoveFromCartCommand, ApiResponse<string>>,
        IRequestHandler<UpdateItemQuantityCommand, ApiResponse<string>>,
        IRequestHandler<DeleteCartCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
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
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.DeleteCartAsync(request.CartId);
            if (result) return Deleted<string>();
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]);
        }

        public async Task<ApiResponse<string>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.AddToCartAsync(request.ProductId, request.Quantity);
            return result switch
            {
                "Success" => Success<string>(_stringLocalizer[SharedResourcesKeys.AddedToCart]),
                "ProductNotFound" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]),
                "FailedInAddItemToCart" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToModifyThisCart]),
                "ItemAlreadyExistsInCart" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.ItemAlreadyExistsInCart]),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AnErrorOccurredWhileAddingToTheCart])
            };
        }

        public async Task<ApiResponse<string>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.RemoveItemFromCartAsync(request.ProductId);
            return result switch
            {
                "Success" => Success<string>(_stringLocalizer[SharedResourcesKeys.ItemRemovedFromCart]),
                "CartNotFound" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.CartNotFound]),
                "ProductNotFound" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]),
                "ItemNotFoundInCart" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.ItemNotFoundInCart]),
                "FailedInRemoveItemFromCart" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToModifyThisCart]),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AnErrorOccurredWhileRemovingFromTheCart])
            };
        }

        public async Task<ApiResponse<string>> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.UpdateItemQuantityAsync(request.ProductId, request.Quantity);
            return result switch
            {
                "Success" => Success<string>(_stringLocalizer[SharedResourcesKeys.ItemQuantityUpdated]),
                "CartNotFound" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.CartNotFound]),
                "ProductNotFound" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]),
                "ItemNotFoundInCart" => NotFound<string>(_stringLocalizer[SharedResourcesKeys.ItemNotFoundInCart]),
                "FailedInUpdateItemQuantity" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToModifyThisCart]),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AnErrorOccurredWhileUpdatingItemQuantity])
            };
        }
        #endregion
    }
}
