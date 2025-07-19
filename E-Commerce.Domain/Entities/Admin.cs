using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Domain.Entities
{
    public class Admin : User
    {
        public string? Address { get; set; }
    }
}
