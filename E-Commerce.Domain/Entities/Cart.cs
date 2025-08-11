using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Domain.Entities
{
    public class Cart
    {
        [Key]
        public Guid CustomerId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string PaymentToken { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal? TotalAmount { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }
    }
}
