using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.ShippingAddresses.Commands.Models
{
    public record DeleteShippingAddressCommand(Guid Id) : IRequest<ApiResponse<string>>;
}
