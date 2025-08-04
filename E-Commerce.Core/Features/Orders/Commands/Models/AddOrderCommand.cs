using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models;
public class AddOrderCommand : IRequest<ApiResponse<string>>
{
    public Guid CartId { get; set; }
    //public List<OrderItemResult>? OrderItemResults { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public Guid? ShippingAddressId { get; set; }
    public DeliveryMethod? DeliveryMethod { get; set; }
}

//public class OrderItemResult
//{
//    public Guid ProductId { get; set; }
//    public int Quantity { get; set; }
//}