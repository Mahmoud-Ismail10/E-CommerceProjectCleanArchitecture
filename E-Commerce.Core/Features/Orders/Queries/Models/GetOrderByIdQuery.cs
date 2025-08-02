using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Orders.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Orders.Queries.Models;
public record GetOrderByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleOrderResponse>>
{
    public int OrderPageNumber { get; set; }
    public int OrderPageSize { get; set; }
};
