using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Categories.Commands.Models
{
    public record EditCategoryCommand(Guid Id, string Name, string? Description) : IRequest<ApiResponse<string>>;
}