using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.ShippingAddresses.Commands.Models
{
    public record EditShippingAddressCommand
    (Guid Id,
        string? FirstName,
        string? LastName,
        string? Street,
        string? City,
        string? State) : IRequest<ApiResponse<string>>;
}
