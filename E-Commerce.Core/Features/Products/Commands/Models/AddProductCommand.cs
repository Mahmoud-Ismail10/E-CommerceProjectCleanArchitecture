using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Products.Commands.Models
{
    public record AddProductCommand : IRequest<ApiResponse<string>>
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public int? StockQuantity { get; init; }
        public string? ImageURL { get; init; }
        public Guid CategoryId { get; init; }
    }
}
