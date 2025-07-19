using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Domain.Entities
{
    public class Employee : User
    {
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Address { get; set; }
    }
}
