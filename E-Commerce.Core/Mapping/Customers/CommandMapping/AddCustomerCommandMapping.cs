using E_Commerce.Core.Features.ApplicationUser.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Customers
{
    public partial class CustomerProfile
    {
        public void AddCustomerCommandMapping()
        {
            CreateMap<AddCustomerCommand, Customer>();
        }
    }
}
