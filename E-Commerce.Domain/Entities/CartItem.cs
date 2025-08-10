namespace E_Commerce.Domain.Entities
{
    public class CartItem
    {
        /// Composite Primary Key <CartId, ProductId>
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; } // Navigation property to Cart
        public Guid ProductId { get; set; }
        public Product? Product { get; set; } // Navigation property to Product

        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? SubAmount => Price * Quantity;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
