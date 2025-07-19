using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Entities
{
    public class Review
    {
        /// Composite Primary Key <CustomerId, ProductId>
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public Rating? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
