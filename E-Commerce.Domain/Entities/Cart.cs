namespace E_Commerce.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }
    }
}
