using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Customers.Queries.Responses
{
    public record GetCustomerPaginatedListResponse
    {
        public Guid Id { get; init; }
        public string? FullName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public Gender? Gender { get; init; }
    }
}
