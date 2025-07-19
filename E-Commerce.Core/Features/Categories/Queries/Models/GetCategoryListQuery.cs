using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Categories.Queries.Response;
using MediatR;

namespace E_Commerce.Core.Features.Categories.Queries.Models
{
    public record GetCategoryListQuery : IRequest<ApiResponse<List<GetCategoryListResponse>>>;
}
