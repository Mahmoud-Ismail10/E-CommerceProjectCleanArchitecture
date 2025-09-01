namespace E_Commerce.Core.Features.ShippingAddresses.Queries.Responses
{
    public record GetSingleShippingAddressResponse(Guid Id, string? FirstName, string? LastName, string? Street, string? City, string? State);
}
