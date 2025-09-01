using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models
{
    public record AddOrderCommand : IRequest<Guid>;
}
//{
//public PaymentMethod? PaymentMethod { get; set; }
//public Guid? ShippingAddressId { get; set; }
//public DeliveryMethod? DeliveryMethod { get; set; }
//}