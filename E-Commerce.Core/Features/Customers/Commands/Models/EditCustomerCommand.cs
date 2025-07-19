using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Customers.Commands.Models
{
    public record EditCustomerCommand : IRequest<ApiResponse<string>>
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public Gender? Gender { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
