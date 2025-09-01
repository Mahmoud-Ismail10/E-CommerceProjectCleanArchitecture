using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Service.Services.Contract
{
    public interface IOrderService
    {
        Task<IReadOnlyList<Order>> GetOrderListAsync();
        IQueryable<Order> FilterOrderPaginatedQueryable(OrderSortingEnum sortBy, string search);
        IQueryable<Order> FilterOrderPaginatedByCustomerIdQueryable(OrderSortingEnum sortBy, string search, Guid customerId);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<string> AddOrderAsync(Order order);
        Task<string> PlaceOrderAsync(Order order);
        Task<string> EditOrderAsync(Order order);
        Task<string> DeleteOrderAsync(Order order);
        Task<bool> IsOrderIdExist(Guid id);
        Task<Order?> GetLatestOrderForUserAsync(Guid userId);
    }
}
