using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;

namespace E_Commerce.Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepositoryAsync<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(E_CommerceContext dbContext) : base(dbContext)
        {
        }
    }
}
