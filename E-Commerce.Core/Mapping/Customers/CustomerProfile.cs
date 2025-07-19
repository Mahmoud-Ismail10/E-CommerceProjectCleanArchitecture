using AutoMapper;

namespace E_Commerce.Core.Mapping.Customers
{
    public partial class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            AddCustomerCommandMapping();
            EditCustomerCommandMapping();
            GetCustomerByIdQueryMapping();
        }
    }
}
