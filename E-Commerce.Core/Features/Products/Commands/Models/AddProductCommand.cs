using E_Commerce.Core.Bases;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.Features.Products.Commands.Models
{
    public record AddProductCommand
    (
        string? Name,
        string? Description,
        decimal? Price,
        int? StockQuantity,
        IFormFile? ImageURL,
        Guid CategoryId
    ) : IRequest<ApiResponse<string>>;
}
