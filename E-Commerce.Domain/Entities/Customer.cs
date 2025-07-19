using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Domain.Entities
{
    public class Customer : User
    {
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShippingAddress> ShippingAddresses { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public Customer()
        {
            Orders = new HashSet<Order>();
            ShippingAddresses = new HashSet<ShippingAddress>();
            Reviews = new HashSet<Review>();
        }
    }
}
