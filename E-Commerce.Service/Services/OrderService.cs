using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Service.Services
{
    public class OrderService : IOrderService
    {
        #region Fields
        private readonly IOrderRepository _orderRepository;
        #endregion

        #region Constructors
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        #endregion

        #region handle Functions
        public Task<string> AddOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<string> EditOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Order> FilterOrderPaginatedQueryable(OrderSortingEnum sortBy, string search)
        {
            var queryable = _orderRepository.GetTableNoTracking()
                                            .Include(c => c.Customer)
                                            .Include(c => c.ShippingAddress)
                                            .Include(c => c.Payment)
                                            .Include(c => c.Delivery).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                queryable = queryable.Where(c => c.Customer!.FirstName!.Contains(search)
                                              || c.Customer!.LastName!.Contains(search)
                                              || c.ShippingAddress!.City!.Contains(search)
                                              || c.ShippingAddress!.Street!.Contains(search)
                                              || c.ShippingAddress!.State!.Contains(search));

            queryable = sortBy switch
            {
                OrderSortingEnum.OrderDateAsc => queryable.OrderBy(c => c.OrderDate),
                OrderSortingEnum.OrderDateDesc => queryable.OrderByDescending(c => c.OrderDate),
                OrderSortingEnum.TotalAmountAsc => queryable.OrderBy(c => c.TotalAmount),
                OrderSortingEnum.TotalAmountDesc => queryable.OrderByDescending(c => c.TotalAmount),
                _ => queryable.OrderBy(c => c.OrderDate)
            };
            return queryable;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetTableNoTracking()
                                              .Where(c => c.Id.Equals(id))
                                              .Include(c => c.Customer)
                                              .Include(c => c.ShippingAddress)
                                              .Include(c => c.Payment)
                                              .Include(c => c.Delivery)
                                              .FirstOrDefaultAsync();
            return order;
        }

        public Task<IReadOnlyList<Order>> GetOrderListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsOrderIdExist(Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
