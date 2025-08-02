using E_Commerce.Core.Features.ShippingAddresses.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.ShippingAddresses
{
    public partial class ShippingAddressProfile
    {
        public void AddShippingAddressCommandMapping()
        {
            CreateMap<AddShippingAddressCommand, ShippingAddress>();
        }
    }
}
