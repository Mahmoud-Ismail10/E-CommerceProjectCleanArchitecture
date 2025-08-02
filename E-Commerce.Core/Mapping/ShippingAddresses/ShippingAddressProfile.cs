using AutoMapper;

namespace E_Commerce.Core.Mapping.ShippingAddresses
{
    public partial class ShippingAddressProfile : Profile
    {
        public ShippingAddressProfile()
        {
            AddShippingAddressCommandMapping();
        }
    }
}
