using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Orders.Queries.Responses
{
    public record GetOrderPaginatedListResponse(
        Guid Id,
        DateTime? OrderDate,
        Status? OrderStatus,
        decimal? TotalAmount,
        string? CustomerName,
        string? ShippingAddress,
        PaymentMethod? PaymentMethod,
        DateTime? PaymentDate,
        Status? PaymentStatus,
        DeliveryMethod? DeliveryMethod,
        DateTime? DeliveryTime,
        decimal? DeliveryCost);
}
