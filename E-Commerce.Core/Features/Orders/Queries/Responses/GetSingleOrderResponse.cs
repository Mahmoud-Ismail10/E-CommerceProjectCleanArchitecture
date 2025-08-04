using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Orders.Queries.Responses
{
    public record class GetSingleOrderResponse()
    {
        public Guid Id { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public Status? OrderStatus { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? CustomerName { get; set; }
        public string? ShippingAddress { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public Status? PaymentStatus { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public DateTimeOffset? DeliveryTime { get; set; }
        public decimal? DeliveryCost { get; set; }

        public PaginatedResult<OrderItemResponse>? OrderItems { get; set; }
    }

    public record class OrderItemResponse(
        Guid ProductId,
        string? ProductName,
        int? Quantity,
        decimal? UnitPrice);
}