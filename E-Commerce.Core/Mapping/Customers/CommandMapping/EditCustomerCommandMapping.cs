using E_Commerce.Core.Features.Customers.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Customers
{
    public partial class CustomerProfile
    {
        public void EditCustomerCommandMapping()
        {
            CreateMap<EditCustomerCommand, Customer>();
        }
    }
}
