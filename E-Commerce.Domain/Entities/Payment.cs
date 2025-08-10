using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string? TransactionId { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public decimal? TotalAmount { get; set; }
        public Status? Status { get; set; }
    }
}
