namespace E_Commerce.Core.Features.Carts.Queries.Responses
{
    public class GetSingleCartResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public Guid CustomerId { get; set; }
        public List<CartItemOfGetSingleResponse>? CartItems { get; set; }
    }

    public class CartItemOfGetSingleResponse
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
