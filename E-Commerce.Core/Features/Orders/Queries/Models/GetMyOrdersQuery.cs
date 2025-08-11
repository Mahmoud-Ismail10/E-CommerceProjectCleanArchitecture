using E_Commerce.Core.Features.Orders.Queries.Responses;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Queries.Models
{
    public record GetMyOrdersQuery(int PageNumber, int PageSize, string? Search,
        OrderSortingEnum SortBy) : IRequest<PaginatedResult<GetMyOrdersResponse>>;
}
