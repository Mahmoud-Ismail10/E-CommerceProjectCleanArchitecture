using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Employees.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Employees.Queries.Models
{
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleEmployeeResponse>>;
}
