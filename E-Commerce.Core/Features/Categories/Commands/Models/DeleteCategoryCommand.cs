using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Categories.Commands.Models
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
