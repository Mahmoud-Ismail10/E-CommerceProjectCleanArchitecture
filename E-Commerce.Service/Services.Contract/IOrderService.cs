using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Service.Services.Contract
{
    public interface IOrderService
    {
        Task<IReadOnlyList<Order>> GetOrderListAsync();
        IQueryable<Order> FilterOrderPaginatedQueryable(OrderSortingEnum sortBy, string search);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<string> AddOrderAsync(Order order);
        Task<string> EditOrderAsync(Order order);
        Task<string> DeleteOrderAsync(Order order);
        Task<bool> IsOrderIdExist(Guid id);
    }
}
