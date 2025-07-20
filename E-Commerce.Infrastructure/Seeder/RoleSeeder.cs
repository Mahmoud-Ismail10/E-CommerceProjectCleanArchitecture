using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            var rolesCount = await roleManager.Roles.CountAsync();
            if (rolesCount <= 0)
            {
                await roleManager.CreateAsync(new Role() { Name = "Admin" });
                await roleManager.CreateAsync(new Role() { Name = "Employee" });
                await roleManager.CreateAsync(new Role() { Name = "Customer" });
            }
        }
    }
}
