using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface ICartService
    {
        Task<string> AddToCartAsync(Guid productId, int quantity);
        Task<string> RemoveItemFromCartAsync(Guid productId);
        Task<string> UpdateItemQuantityAsync(Guid productId, int Quantity);
        Task<Cart?> GetCartByKeyAsync(string cartKey);
        Task<Cart?> GetMyCartAsync();
        Task<Cart?> AddOrEditCartAsync(Cart cart); // Add or Edit
        Task<bool> DeleteMyCartAsync();
        Task<bool> DeleteCartAsync(Guid customerId);
        Task<string> MigrateGuestCartToCustomerAsync(Guid customerId);
    }
}
