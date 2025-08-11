using E_Commerce.Domain.Entities;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.AspNetCore.Http;
using Serilog;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Service.Services
{
    public class CartService : ICartService
    {
        #region Fields
        private readonly IDatabase _database;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public CartService(IDatabase database,
            ICurrentUserService currentUserService,
            IProductService productService,
            IHttpContextAccessor httpContextAccessor,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
            _currentUserService = currentUserService;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Private Helpers
        private string GetCartKey() => $"cart:{_currentUserService.GetCartOwnerId()}";
        #endregion

        #region Handle Functions
        public async Task<Cart?> GetCartByKeyAsync(string cartKey)
        {
            var cart = await _database.StringGetAsync(cartKey); // return JSON
            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cart!); // Convert JSON to Object of Cart
        }

        // Add or Update Cart
        public async Task<Cart?> AddOrEditCartAsync(Cart cart)
        {
            var cartKey = $"cart:{cart.CustomerId}";
            var existingCart = await GetCartByKeyAsync(cartKey);

            if (existingCart is not null)
            {
                cart.CreatedAt = cart.CreatedAt == default ? existingCart.CreatedAt : DateTimeOffset.UtcNow.ToLocalTime();
                cart.CustomerId = cart.CustomerId == Guid.Empty ? existingCart.CustomerId : cart.CustomerId;
                cart.CartItems = cart.CartItems ?? existingCart.CartItems;
                cart.TotalAmount = cart.TotalAmount ?? existingCart.TotalAmount;
                cart.PaymentToken = string.IsNullOrEmpty(cart.PaymentToken) ? existingCart.PaymentToken : cart.PaymentToken;
                cart.PaymentIntentId = string.IsNullOrEmpty(cart.PaymentIntentId) ? existingCart.PaymentIntentId : cart.PaymentIntentId;
            }

            var createOrUpdateCart = await _database.StringSetAsync(cartKey, JsonSerializer.Serialize(cart), TimeSpan.FromDays(3));
            if (createOrUpdateCart is false) return null;
            return await GetCartByKeyAsync(cartKey);
        }

        public async Task<Cart?> GetMyCartAsync()
        {
            var cartKey = GetCartKey();
            var cart = await _database.StringGetAsync(cartKey);
            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cart!);
        }

        public async Task<string> AddToCartAsync(Guid productId, int quantity)
        {
            try
            {
                var ownerId = _currentUserService.GetCartOwnerId();
                var cartKey = $"cart:{ownerId}";
                var existingCart = await GetCartByKeyAsync(cartKey) ?? new Cart
                {
                    CustomerId = ownerId,
                    CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
                    CartItems = new List<CartItem>(),
                    TotalAmount = 0,
                };
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(productId);
                if (existingProduct is null) return "ProductNotFound";
                // Check if the item already exists in the cart
                var existingItem = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == productId);
                if (existingItem != null)
                    return "ItemAlreadyExistsInCart";
                else
                {
                    // Add new item to the cart
                    existingCart.CartItems!.Add(new CartItem
                    {
                        CartId = existingCart.CustomerId,
                        ProductId = productId,
                        Price = existingProduct.Price,
                        Quantity = quantity,
                        CreatedAt = DateTimeOffset.UtcNow.ToLocalTime(),
                        SubAmount = existingProduct.Price * quantity
                    });
                    existingCart.TotalAmount += existingProduct.Price * quantity;
                }
                // Save the updated cart
                var result = await AddOrEditCartAsync(existingCart);
                if (result is null) return "FailedInAddItemToCart";
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error adding item to cart: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "AnErrorOccurredWhileAddingToTheCart";
            }
        }

        public async Task<string> RemoveItemFromCartAsync(Guid productId)
        {
            try
            {
                var existingCart = await GetMyCartAsync();
                if (existingCart is null) return "CartNotFound";
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(productId);
                if (existingProduct is null) return "ProductNotFound";
                // Find the item to remove
                var itemToRemove = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == productId);
                if (itemToRemove is null) return "ItemNotFoundInCart";
                // Remove the item
                existingCart.CartItems!.Remove(itemToRemove);
                // Save the updated cart
                var result = await AddOrEditCartAsync(existingCart);
                if (result is null) return "FailedInRemoveItemFromCart";
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error removing item from cart: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "AnErrorOccurredWhileRemovingItemFromTheCart";
            }
        }

        public async Task<string> UpdateItemQuantityAsync(Guid productId, int Quantity)
        {
            try
            {
                var existingCart = await GetMyCartAsync();
                if (existingCart is null) return "CartNotFound";
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(productId);
                if (existingProduct is null) return "ProductNotFound";
                // Find the item to update
                var itemToUpdate = existingCart.CartItems?.FirstOrDefault(x => x.ProductId == productId);
                if (itemToUpdate is null) return "ItemNotFoundInCart";
                // Update the quantity
                itemToUpdate.Quantity = Quantity;
                // Save the updated cart
                var result = await AddOrEditCartAsync(existingCart);
                if (result is null) return "FailedInUpdateItemQuantity";
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error updating item quantity in cart: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "AnErrorOccurredWhileUpdatingItemQuantityInTheCart";
            }
        }

        public async Task<bool> DeleteMyCartAsync()
        {
            try
            {
                var cartKey = GetCartKey();
                return await _database.KeyDeleteAsync(cartKey);
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting cart: {Message}", ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCartAsync(Guid customerId)
        {
            try
            {
                var cartKey = $"cart:{customerId}";
                return await _database.KeyDeleteAsync(cartKey);
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting cart: {Message}", ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }

        public async Task<string> MigrateGuestCartToCustomerAsync(Guid customerId)
        {
            var transaction = _database.CreateTransaction();
            try
            {
                var guestId = _httpContextAccessor.HttpContext?.Request.Cookies["GuestId"];

                var guestCartKey = $"cart:{guestId}";
                var userCartKey = $"cart:{customerId}";

                var guestCart = await GetCartByKeyAsync(guestCartKey);
                if (guestCart != null)
                {
                    guestCart.CustomerId = customerId;
                    var result1 = await AddOrEditCartAsync(guestCart);
                    if (result1 is null) return "FailedInEditCart";
                    await DeleteCartAsync(Guid.Parse(guestId!));
                }

                // Delete the guest id from Cookies
                var result2 = _currentUserService.DeleteGuestIdCookie();
                if (!result2)
                {
                    Log.Error("Failed to delete GuestId cookie.");
                    return "FailedToDeleteGuestIdCookie";
                }

                var committed = await transaction.ExecuteAsync();
                if (!committed)
                {
                    Log.Error("Transaction failed to commit while migrating guest cart to customer.");
                    return "TransactionFailedToCommit";
                }
                else
                    return "Success";
            }
            catch (Exception)
            {
                Log.Error("Error migrating guest cart to customer cart.");
                return "FailedInMigrateGuestCartToCustomer";
            }
        }
        #endregion
    }
}
