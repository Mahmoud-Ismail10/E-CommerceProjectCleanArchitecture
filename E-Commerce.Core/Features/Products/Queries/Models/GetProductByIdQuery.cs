using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Products.Queries.Responses;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Products.Queries.Models
{
    public record GetProductByIdQuery(Guid ProductId, int ReviewPageNumber, int ReviewPageSize,
        ReviewSortingEnum SortBy, string? Search) : IRequest<ApiResponse<GetSingleProductResponse>>;
}
