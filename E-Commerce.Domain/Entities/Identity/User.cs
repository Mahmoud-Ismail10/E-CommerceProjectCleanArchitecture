using E_Commerce.Domain.Enums;
using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Domain.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }

        [EncryptColumn]
        public string? Code { get; set; }
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

        public User()
        {
            UserRefreshTokens = new HashSet<UserRefreshToken>();
        }
    }
}
