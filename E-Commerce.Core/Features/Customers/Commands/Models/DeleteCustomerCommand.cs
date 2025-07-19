using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Customers.Commands.Models
{
    public record DeleteCustomerCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
