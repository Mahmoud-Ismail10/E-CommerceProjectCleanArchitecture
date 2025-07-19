namespace E_Commerce.Domain.Entities
{
    public class OrderItem
    {
        /// Composite Primary Key <ProductId, OrderId>
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
