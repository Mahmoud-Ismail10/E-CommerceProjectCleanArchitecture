using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface ICartService
    {
        Task<Cart?> GetCartByIdAsync(Guid cartId);
        Task<Cart?> UpdateCartAsync(Cart cart); // Add or Update
        Task<bool> DeleteCartAsync(Guid cartId);
    }
}
