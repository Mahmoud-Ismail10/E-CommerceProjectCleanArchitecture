using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models
{
    public record DeleteOrderCommand(Guid OrderId) : IRequest<ApiResponse<string>>;
}
