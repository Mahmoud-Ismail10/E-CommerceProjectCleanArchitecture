using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Carts.Commands.Models
{
    public record UpdateItemQuantityCommand(Guid ProductId, int Quantity) : IRequest<ApiResponse<string>>;
}
