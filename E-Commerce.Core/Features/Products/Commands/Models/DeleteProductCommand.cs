using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Products.Commands.Models
{
    public record DeleteProductCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
