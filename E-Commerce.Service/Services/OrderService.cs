using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Domain.Helpers;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class OrderService : IOrderService
    {
        #region Fields
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IDeliveryService _deliveryService;
        private readonly ICartService _cartService;
        private readonly IShippingAddressService _shippingAddressService;
        private readonly IOrderItemRepository _orderItemRepository;
        #endregion

        #region Constructors
        public OrderService(IOrderRepository orderRepository,
            IProductService productService,
            IDeliveryService deliveryService,
            ICartService cartService,
            IShippingAddressService shippingAddressService,
            IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _deliveryService = deliveryService;
            _cartService = cartService;
            _shippingAddressService = shippingAddressService;
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region handle Functions
        public async Task<string> AddOrderAsync(Order order)
        {
            using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                // Discount quantity from stock
                foreach (var item in order.OrderItems)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        var result1 = await _productService.EditProductAsync(product);
                        if (result1 != "Success")
                        {
                            await transaction.RollbackAsync();
                            return "FailedInDiscountQuantityFromStock"; // Return error message from product service
                        }
                    }
                }

                // Delivery Settings
                if (order.Delivery!.DeliveryMethod != DeliveryMethod.PickupFromBranch)
                {
                    var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(order.ShippingAddressId);
                    if (shippingAddress == null)
                    {
                        await transaction.RollbackAsync();
                        return "ShippingAddressDoesNotExist";
                    }

                    var deliveryOffset = DeliveryTimeCalculator.Calculate(shippingAddress.City, order.Delivery.DeliveryMethod);
                    var deliveryCost = DeliveryCostCalculator.Calculate(shippingAddress.City, order.Delivery.DeliveryMethod);

                    order.Delivery.Id = Guid.NewGuid();
                    order.Delivery.Description = $"Delivery for order {order.Id} to {shippingAddress.State}, {shippingAddress.City}, {shippingAddress.Street}";
                    order.Delivery.DeliveryTime = DateTime.UtcNow.Add(deliveryOffset);
                    order.Delivery.Cost = deliveryCost;
                    order.Delivery.Status = Status.Pending;
                    order.DeliveryId = order.Delivery.Id;
                    _orderRepository.AttachEntity(shippingAddress);
                    //order.ShippingAddress = shippingAddress;
                }
                else
                    order.Delivery = null;

                await _orderRepository.AddAsync(order);
                //foreach (var item in order.OrderItems)
                //{
                //    item.OrderId = order.Id;
                //}

                //await _orderItemRepository.AddRangeAsync(order.OrderItems);

                var result2 = await _cartService.DeleteMyCartAsync();
                if (!result2)
                {
                    await transaction.RollbackAsync();
                    return "FailedInDeletingCart";
                }
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error adding order: {Message}", ex.InnerException?.Message ?? ex.Message);
                return "FailedInAdd";
            }
        }

        public async Task<string> DeleteOrderAsync(Order order)
        {
            try
            {
                await _orderRepository.DeleteAsync(order);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting order {OrderId}: {ErrorMessage}", order.Id, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<string> EditOrderAsync(Order order)
        {
            try
            {
                await _orderRepository.UpdateAsync(order);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error editing order {OrderId}: {ErrorMessage}", order.Id, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
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
                _ => queryable.OrderByDescending(c => c.OrderDate)
            };
            return queryable;
        }

        public IQueryable<Order> FilterOrderPaginatedByCustomerIdQueryable(OrderSortingEnum sortBy, string search, Guid customerId)
        {
            var queryable = _orderRepository.GetTableNoTracking().Where(o => o.CustomerId == customerId)
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
                _ => queryable.OrderByDescending(c => c.OrderDate)
            };
            return queryable;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetTableAsTracking()
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

        public async Task<Order?> GetLatestOrderForUserAsync(Guid userId)
        {
            return await _orderRepository.GetTableAsTracking()
                                   .Where(o => o.CustomerId == userId)
                                   .OrderByDescending(o => o.OrderDate)
                                   .FirstOrDefaultAsync();
        }
        #endregion
    }
}
