using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? Amount { get; set; }
        public Status? Status { get; set; }
    }
}
