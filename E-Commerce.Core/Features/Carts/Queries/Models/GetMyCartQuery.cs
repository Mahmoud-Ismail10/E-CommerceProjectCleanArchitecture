using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Carts.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Carts.Queries.Models
{
    public record GetMyCartQuery : IRequest<ApiResponse<GetSingleCartResponse>>;
}
