using MediatR;

namespace E_Commerce.Core.Features.Orders.Commands.Models
{
    public record AddOrderCommand : IRequest<Guid>;
}