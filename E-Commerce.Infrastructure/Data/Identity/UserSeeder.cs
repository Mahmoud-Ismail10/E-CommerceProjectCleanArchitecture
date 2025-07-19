using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Identity
{
    //public static class UserSeeder
    //{
    //    public static async Task AddSuperAdminAsync(UserManager<User> _userManager)
    //    {
    //        if (_userManager.Users.Count() == 0)
    //        {
    //            var Admin = new User()
    //            {
    //                FirstName = "Mahmoud",
    //                LastName = "Ismail",
    //                Email = "mahmoud.ismail1872@gmail.com",
    //                PhoneNumber = "01002876238",
    //                Gender = Gender.Male,
    //                UserRole = UserRole.Admin
    //            };
    //            var createResult = await _userManager.CreateAsync(Admin, superAdmin1.NationalId);
    //            if (createResult.Succeeded)
    //            {
    //                var roleResult = await _userManager.AddToRoleAsync(Admin, AuthorizationConstants.SuperAdminRole);
    //                if (!roleResult.Succeeded)
    //                    throw new Exception($"Failed to assign Admin role " +
    //                        $": {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
    //            }
    //            else
    //            {
    //                throw new Exception($"Failed to create Admin " +
    //                    $": {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
    //            }
    //        }
    //    }
    //}
}
