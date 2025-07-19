using E_Commerce.Core.Features.Customers.Queries.Responses;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Customers.Queries.Models
{
    public record GetCustomerPaginatedListQuery(int PageNumber, int PageSize, string? Search,
    CustomerSortingEnum SortBy) : IRequest<PaginatedResult<GetCustomerPaginatedListResponse>>;
}
