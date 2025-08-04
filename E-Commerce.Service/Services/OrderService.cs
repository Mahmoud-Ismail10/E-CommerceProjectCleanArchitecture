using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
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
        private readonly IPaymentService _paymentService;
        private readonly IDeliveryService _deliveryService;
        private readonly ICartService _cartService;
        private readonly IOrderItemRepository _orderItemRepository;
        #endregion

        #region Constructors
        public OrderService(IOrderRepository orderRepository,
            IProductService productService,
            IPaymentService paymentService,
            IDeliveryService deliveryService,
            ICartService cartService,
            IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _paymentService = paymentService;
            _deliveryService = deliveryService;
            _cartService = cartService;
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region handle Functions
        public async Task<string> AddOrderAsync(Order order, List<OrderItem> orderItems, Payment payment, Delivery? delivery, Guid cartId)
        {
            using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                // Discount quantity from stock
                foreach (var item in orderItems)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        var result = await _productService.EditProductAsync(product);
                        if (result != "Success")
                        {
                            await transaction.RollbackAsync();
                            return "FailedInDiscountQuantityFromStock"; // Return error message from product service
                        }
                    }
                }

                // Payment processing
                var paymentResult = await _paymentService.AddPaymentAsync(payment);
                if (paymentResult != "Success")
                {
                    await transaction.RollbackAsync();
                    return "FailedInPaymentProcessing";
                }
                order.PaymentId = payment.Id;

                // Delivery processing if exists
                if (delivery is not null)
                {
                    var deliveryResult = await _deliveryService.AddDeliveryAsync(delivery);
                    if (deliveryResult != "Success")
                    {
                        await transaction.RollbackAsync();
                        return "FailedInDeliveryProcessing";
                    }
                    order.DeliveryId = delivery.Id;
                }

                await _orderRepository.AddAsync(order);
                foreach (var item in orderItems)
                {
                    item.OrderId = order.Id;
                }
                await _orderItemRepository.AddRangeAsync(orderItems);
                await _cartService.DeleteCartAsync(cartId);

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
