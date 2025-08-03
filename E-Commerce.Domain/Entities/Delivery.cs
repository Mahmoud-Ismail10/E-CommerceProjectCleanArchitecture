using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Entities
{
    public class Delivery
    {
        public Guid Id { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public string? Description { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public decimal? Cost { get; set; }
    }
}
