using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.ShippingAddresses.Commands.Models
{
    public class AddShippingAddressCommand : IRequest<ApiResponse<string>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public Guid? CustomerId { get; set; }
    }

}
