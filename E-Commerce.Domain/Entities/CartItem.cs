namespace E_Commerce.Domain.Entities
{
    public class CartItem
    {
        /// Composite Primary Key <CartId, ProductId>
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }

        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? SubAmount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
