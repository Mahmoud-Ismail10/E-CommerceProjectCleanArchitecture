namespace E_Commerce.Domain.Entities
{
    public class DeliveryMethod
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public decimal? Cost { get; set; }
    }
}
