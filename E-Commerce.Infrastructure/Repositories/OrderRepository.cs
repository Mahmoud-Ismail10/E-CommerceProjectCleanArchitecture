using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepositoryAsync<Order>, IOrderRepository
    {
        #region Fields
        private readonly DbSet<Order> _orders;
        #endregion

        #region Constructors
        public OrderRepository(E_CommerceContext dbContext) : base(dbContext)
        {
            _orders = dbContext.Set<Order>();
        }
        #endregion

        #region Handle Functions

        #endregion
    }
}
