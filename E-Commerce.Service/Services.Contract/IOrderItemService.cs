using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface IOrderItemService
    {
        IQueryable<OrderItem> GetOrderItemsByOrderIdQueryable(Guid orderId);
    }
}
