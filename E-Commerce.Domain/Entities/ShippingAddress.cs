namespace E_Commerce.Domain.Entities
{
    public class ShippingAddress
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
