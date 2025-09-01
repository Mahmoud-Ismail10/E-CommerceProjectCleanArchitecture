using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Reviews.Queries.Responses;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Reviews.Queries.Models
{
    public record GetReviewPaginatedListQuery(Guid ProductId, int PageNumber, int PageSize, string? Search,
        ReviewSortingEnum SortBy) : IRequest<ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>>;
}
