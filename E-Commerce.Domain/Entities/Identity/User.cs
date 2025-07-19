using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Domain.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new List<UserRefreshToken>();

        public User()
        {
            UserRefreshTokens = new List<UserRefreshToken>();
        }
    }
}
