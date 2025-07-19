using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Employees.Queries.Responses
{
    public record GetSingleEmployeeResponse
    {
        public Guid Id { get; init; }
        public string? FullName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public Gender? Gender { get; init; }
        public string? Position { get; init; }
        public decimal? Salary { get; init; }
        public DateTime? HireDate { get; init; }
        public string? Address { get; init; }
    }
}
