using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.ShippingAddresses.Commands.Models
{
    public record SetShippingAddressCommand(Guid OrderId, Guid ShippingAddressId) : IRequest<ApiResponse<string>>;
}
