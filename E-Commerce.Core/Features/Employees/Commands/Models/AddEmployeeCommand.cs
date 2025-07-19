using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Employees.Commands.Models
{
    public record AddEmployeeCommand : IRequest<ApiResponse<string>>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public Gender? Gender { get; init; }
        public string? PhoneNumber { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
        public string Position { get; init; }
        public decimal Salary { get; init; }
        public string Address { get; init; }
    }
}
