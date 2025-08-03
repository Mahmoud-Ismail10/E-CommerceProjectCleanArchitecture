using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;

namespace E_Commerce.Infrastructure.Repositories
{
    public class DeliveryRepository : GenericRepositoryAsync<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(E_CommerceContext dbContext) : base(dbContext)
        {
        }
    }
}
