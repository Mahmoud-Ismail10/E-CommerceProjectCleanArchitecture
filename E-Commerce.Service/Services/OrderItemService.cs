using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Service.Services
{
    public class OrderItemService : IOrderItemService
    {
        #region Fields
        private readonly IOrderItemRepository _orderItemRepository;
        #endregion

        #region Constructors
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region Handle Functions
        public IQueryable<OrderItem> GetOrderItemsByOrderIdQueryable(Guid orderId)
        {
            return _orderItemRepository.GetTableNoTracking()
                .Where(r => r.OrderId.Equals(orderId))
                .Include(r => r.Product)
                .AsQueryable();
        }
        #endregion
    }
}
