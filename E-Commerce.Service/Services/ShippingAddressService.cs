using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class ShippingAddressService : IShippingAddressService
    {
        #region Fields
        private readonly IShippingAddressRepository _shippingAddressRepository;
        #endregion

        #region Constructors
        public ShippingAddressService(IShippingAddressRepository shippingAddressRepository)
        {
            _shippingAddressRepository = shippingAddressRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddShippingAddressAsync(ShippingAddress shippingAddress)
        {
            try
            {
                await _shippingAddressRepository.AddAsync(shippingAddress);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error adding shipping address : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "FailedInAdd";
            }
        }

        public async Task<IReadOnlyList<ShippingAddress?>> GetShippingAddressListByCustomerIdAsync(Guid customerId)
        {
            return await _shippingAddressRepository.GetTableNoTracking()
                                              .Where(c => c.CustomerId.Equals(customerId))
                                              .ToListAsync();
        }

        public async Task<string> DeleteShippingAddressAsync(ShippingAddress shippingAddress)
        {
            try
            {
                await _shippingAddressRepository.DeleteAsync(shippingAddress);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting shipping address : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "FailedInDelete";
            }
        }

        public async Task<string> EditShippingAddressAsync(ShippingAddress shippingAddress)
        {
            try
            {
                await _shippingAddressRepository.UpdateAsync(shippingAddress);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error updating shipping address: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedInUpdate";
            }
        }

        public async Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid? id)
        {
            var shippingAddress = await _shippingAddressRepository.GetTableNoTracking()
                                              .Where(c => c.Id.Equals(id))
                                              .Include(c => c.Customer)
                                              .FirstOrDefaultAsync();
            return shippingAddress;
        }

        public async Task<bool> IsShippingAddressExist(string street, string city, string state)
        {
            return await _shippingAddressRepository.GetTableNoTracking()
                                              .AnyAsync(c => c.Street == street && c.City == city && c.State == state);
        }

        public async Task<bool> IsShippingAddressExistExcludeSelf(string street, string city, string state, Guid id)
        {
            var category = await _shippingAddressRepository.GetTableNoTracking()
                                              .Where(c => c.Street == street && c.City == city && c.State == state && !c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            if (category != null) return true;
            return false;
        }
        #endregion
    }
}
