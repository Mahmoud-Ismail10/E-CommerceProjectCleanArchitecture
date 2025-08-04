using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Carts.Commands.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Carts.Commands.Models
{
    public record EditCartCommand(EditCartResponse Cart) : IRequest<ApiResponse<EditCartResponse>>;
}
