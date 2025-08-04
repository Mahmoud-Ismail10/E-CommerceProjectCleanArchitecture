using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Orders.Queries.Responses
{
    public record GetOrderPaginatedListResponse(
        Guid Id,
        DateTimeOffset? OrderDate,
        Status? OrderStatus,
        decimal? TotalAmount,
        string? CustomerName,
        string? ShippingAddress,
        PaymentMethod? PaymentMethod,
        DateTimeOffset? PaymentDate,
        Status? PaymentStatus,
        DeliveryMethod? DeliveryMethod,
        DateTimeOffset? DeliveryTime,
        decimal? DeliveryCost);
}
