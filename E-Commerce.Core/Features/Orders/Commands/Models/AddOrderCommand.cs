using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models;
public record AddOrderCommand(
    ) : IRequest<ApiResponse<string>>
{
    public Guid CustomerId { get; init; }
    public DateTime OrderDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string ShippingAddress { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;
    public DateTime PaymentDate { get; init; }
    public string DeliveryMethod { get; init; } = string.Empty;
    public DateTime DeliveryTime { get; init; }
    public decimal DeliveryCost { get; init; }

}
