using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            var usersCount = await userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defualtAdmin = new Admin()
                {
                    FirstName = "Mahmoud",
                    LastName = "Ismail",
                    UserName = "mahmoud.ismail",
                    Gender = Gender.Male,
                    Email = "mahmoud.ismail1872@gmail.com",
                    PhoneNumber = "01002876238",
                    Address = "Egypt, Menofia, ElShohadaa",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(defualtAdmin, "Mahmoud@123");
                await userManager.AddToRoleAsync(defualtAdmin, "Admin");
            }
        }
    }
}
