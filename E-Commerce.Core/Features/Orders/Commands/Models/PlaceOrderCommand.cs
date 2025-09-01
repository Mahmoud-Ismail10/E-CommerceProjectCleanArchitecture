using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Orders.Commands.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models
{
    public record PlaceOrderCommand(Guid OrderId) : IRequest<ApiResponse<PaymentProcessResponse>>;
}
