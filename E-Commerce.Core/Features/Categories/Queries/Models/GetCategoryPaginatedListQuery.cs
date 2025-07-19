using E_Commerce.Core.Features.Categories.Queries.Response;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Categories.Queries.Models
{
    public record GetCategoryPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CategorySortingEnum SortBy) : IRequest<PaginatedResult<GetCategoryPaginatedListResponse>>;
}
