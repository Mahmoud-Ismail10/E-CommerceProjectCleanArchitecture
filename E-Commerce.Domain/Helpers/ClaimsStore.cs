using System.Security.Claims;

namespace E_Commerce.Domain.Helpers
{
    public static class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
            new Claim("Create Customer", "false"),
            new Claim("Edit Customer", "false"),
            new Claim("Delete Customer", "false"),
        };
    }
}
