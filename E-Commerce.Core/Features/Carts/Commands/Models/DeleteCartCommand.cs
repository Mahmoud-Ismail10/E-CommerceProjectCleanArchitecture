using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Carts.Commands.Models
{
    public record DeleteCartCommand(Guid CartId) : IRequest<ApiResponse<string>>;
}
