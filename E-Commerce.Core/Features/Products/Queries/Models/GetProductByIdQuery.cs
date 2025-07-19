using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Products.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Products.Queries.Models
{
    public record GetProductByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleProductResponse>>
    {
        public int ReviewPageNumber { get; init; }
        public int ReviewPageSize { get; init; }
    }
}
