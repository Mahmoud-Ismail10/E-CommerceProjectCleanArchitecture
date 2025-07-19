using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Categories.Commands.Models
{
    public record AddCategoryCommand(string Name, string? Description) : IRequest<ApiResponse<string>>;
}
