namespace E_Commerce.Core.Features.Carts.Commands.Responses
{
    public class EditCartResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public Guid? CustomerId { get; set; }
        public List<CartItemResponse>? CartItems { get; set; }
    }

    public class CartItemResponse
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
