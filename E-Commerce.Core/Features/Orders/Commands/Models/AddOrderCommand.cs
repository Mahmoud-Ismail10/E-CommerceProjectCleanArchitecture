using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models;
public class AddOrderCommand : IRequest<ApiResponse<string>>
{
    public PaymentMethod? PaymentMethod { get; set; }
    public Guid? ShippingAddressId { get; set; }
    public DeliveryMethod? DeliveryMethod { get; set; }
}