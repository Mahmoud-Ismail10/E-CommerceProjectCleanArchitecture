using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.ShippingAddresses.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.ShippingAddresses.Queries.Models
{
    public record GetSingleShippingAddressQuery(Guid Id) : IRequest<ApiResponse<GetSingleShippingAddressResponse>>;
}
