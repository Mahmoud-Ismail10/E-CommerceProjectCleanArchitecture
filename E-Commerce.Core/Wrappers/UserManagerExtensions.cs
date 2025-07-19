using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Core.Wrappers
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> UserNameExistsAsync(
            this UserManager<User> userManager, string userName, Guid currentUserId)
        {
            return await userManager.Users.AnyAsync(u => u.UserName == userName && u.Id != currentUserId);
        }

        public static async Task<bool> EmailExistsAsync(
            this UserManager<User> userManager, string email, Guid currentUserId)
        {
            return await userManager.Users.AnyAsync(u => u.Email == email && u.Id != currentUserId);
        }
    }

}
