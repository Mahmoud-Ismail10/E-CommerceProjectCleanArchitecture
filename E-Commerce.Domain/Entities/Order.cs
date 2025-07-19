using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public Status? Status { get; set; }
        public decimal? TotalAmount { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid ShippingAddressId { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
        public Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public Guid DeliveryMethodId { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
    }
}
