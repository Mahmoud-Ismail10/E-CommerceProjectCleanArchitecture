using E_Commerce.Core.Features.Employees.Queries.Responses;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums.Sorting;
using MediatR;

namespace E_Commerce.Core.Features.Employees.Queries.Models
{
    public record GetEmployeePaginatedListQuery(int PageNumber, int PageSize, string? Search,
    EmployeeSortingEnum SortBy) : IRequest<PaginatedResult<GetEmployeePaginatedListResponse>>;
}
