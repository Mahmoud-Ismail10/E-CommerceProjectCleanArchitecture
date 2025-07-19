using E_Commerce.Core.Features.Customers.Queries.Responses;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Customers
{
    public partial class CustomerProfile
    {
        public void GetCustomerByIdQueryMapping()
        {
            CreateMap<Customer, GetSingleCustomerResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}
