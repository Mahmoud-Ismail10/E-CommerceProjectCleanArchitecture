using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Products.Queries.Responses
{
    public record GetSingleProductResponse
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public int? StockQuantity { get; init; }
        public string? ImageURL { get; init; }
        public DateTime? CreatedAt { get; init; }
        public string? CategoryName { get; init; }

        public PaginatedResult<ReviewResponse>? Reviews { get; set; }
    }

    public record ReviewResponse
    {
        public Guid CustomerId { get; init; }
        public string? FullName { get; init; }
        public Rating? Rating { get; init; }
    }
}
