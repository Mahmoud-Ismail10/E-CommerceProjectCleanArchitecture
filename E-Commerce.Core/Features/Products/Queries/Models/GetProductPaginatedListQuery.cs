using E_Commerce.Core.Features.Products.Queries.Responses;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Products.Queries.Models
{
    public record GetProductPaginatedListQuery(int PageNumber, int PageSize, string? Search,
        ProductSortingEnum SortBy) : IRequest<PaginatedResult<GetProductPaginatedListResponse>>;
}
