namespace E_Commerce.Core.Features.Products.Queries.Responses
{
    public record GetProductPaginatedListResponse
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public int? StockQuantity { get; init; }
        public string? ImageURL { get; init; }
        public DateTimeOffset? CreatedAt { get; init; }
        public string? CategoryName { get; init; }
    }
}
