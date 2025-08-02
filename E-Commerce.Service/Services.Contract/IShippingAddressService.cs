using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface IShippingAddressService
    {
        Task<IReadOnlyList<ShippingAddress?>> GetShippingAddressListAsync();
        Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid id);
        Task<string> AddShippingAddressAsync(ShippingAddress shippingAddress);
        Task<string> EditShippingAddressAsync(ShippingAddress shippingAddress);
        Task<string> DeleteShippingAddressAsync(ShippingAddress shippingAddress);
        Task<bool> IsShippingAddressExist(string street, string city, string state);
        Task<bool> IsShippingAddressExistExcludeSelf(string street, string city, string state, Guid id);
    }
}
