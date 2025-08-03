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

        public Task<string> DeleteShippingAddressAsync(ShippingAddress shippingAddress)
        {
            throw new NotImplementedException();
        }

        public Task<string> EditShippingAddressAsync(ShippingAddress shippingAddress)
        {
            throw new NotImplementedException();
        }

        public Task<ShippingAddress?> GetShippingAddressByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ShippingAddress?>> GetShippingAddressListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsShippingAddressExist(string street, string city, string state)
        {
            return await _shippingAddressRepository.GetTableNoTracking()
                                              .AnyAsync(c => c.Street == street && c.City == city && c.State == state);
        }

        public Task<bool> IsShippingAddressExistExcludeSelf(string street, string city, string state, Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
