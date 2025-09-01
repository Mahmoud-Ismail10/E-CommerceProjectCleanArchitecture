using E_Commerce.Core.Features.ShippingAddresses.Queries.Responses;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.ShippingAddresses
{
    public partial class ShippingAddressProfile
    {
        public void GetSingleShippingAddressQueryMapping()
        {
            CreateMap<ShippingAddress, GetSingleShippingAddressResponse>();
        }
    }
}
