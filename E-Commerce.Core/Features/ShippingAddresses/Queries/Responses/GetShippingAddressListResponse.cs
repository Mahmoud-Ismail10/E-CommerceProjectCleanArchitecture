namespace E_Commerce.Core.Features.ShippingAddresses.Queries.Responses
{
    public record GetShippingAddressListResponse(Guid Id, string? Street, string? City, string? State);
}
