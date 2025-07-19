using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Customers.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Customers.Queries.Models
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleCustomerResponse>>;
}
