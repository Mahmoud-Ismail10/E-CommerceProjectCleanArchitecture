using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Employees.Commands.Models
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
