using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;

namespace E_Commerce.Infrastructure.Repositories
{
    public class ShippingAddressRepository : GenericRepositoryAsync<ShippingAddress>, IShippingAddressRepository
    {
        public ShippingAddressRepository(E_CommerceContext dbContext) : base(dbContext)
        {
        }
    }
}
