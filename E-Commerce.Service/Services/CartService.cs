using E_Commerce.Domain.Entities;
using E_Commerce.Service.Services.Contract;
using Serilog;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Service.Services
{
    public class CartService : ICartService
    {
        #region Fields
        private readonly IDatabase _database;
        #endregion

        #region Constructors
        public CartService(IDatabase database, IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
        #endregion

        #region Handle Functions
        public async Task<bool> DeleteCartAsync(Guid cartId)
        {
            try
            {
                return await _database.KeyDeleteAsync(cartId.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting cart: {Message}", ex.InnerException?.Message ?? ex.Message);
                return false;
            }
        }

        public async Task<Cart?> GetCartByIdAsync(Guid cartId)
        {
            var cart = await _database.StringGetAsync(cartId.ToString()); // return JSON
            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Cart>(cart!); // Convert JSON to Object of Cart
        }

        // Add or Update Cart
        public async Task<Cart?> UpdateCartAsync(Cart cart)
        {
            var existingCart = await GetCartByIdAsync(cart.Id);

            if (existingCart is not null)
            {
                // Merge: only update non-null values from new cart
                cart.CreatedAt = cart.CreatedAt != default ? cart.CreatedAt : existingCart.CreatedAt;
                cart.CustomerId = cart.CustomerId != Guid.Empty ? cart.CustomerId : existingCart.CustomerId;
                cart.CartItems = cart.CartItems?.Any() == true ? cart.CartItems : existingCart.CartItems;
            }

            var createOrUpdateCart = await _database.StringSetAsync(cart.Id.ToString(), JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (createOrUpdateCart is false) return null;
            return await GetCartByIdAsync(cart.Id);
        }
        #endregion
    }
}
