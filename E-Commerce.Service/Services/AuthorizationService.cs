using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Service.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddRoleAsync(string roleName)
        {
            var identityRole = new Role();
            identityRole.Name = roleName;
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded) return "Success";
            return "Failed";
        }

        public async Task<string> DeleteRoleAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null) return "NotFound";
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            if (users is null || users.Count < 0)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded) return "Success";
                var errors = string.Join("-", result.Errors);
                return errors;
            }
            return "Used";
        }

        public async Task<string> EditRoleAsync(string roleName, Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null) return "NotFound";
            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded) return "Success";
            return "Failed";
        }

        public async Task<bool> IsRoleExistByName(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> IsRoleExistById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null) return false;
            return true;
        }

        public async Task<bool> IsRoleExistExcludeSelf(string roleName, Guid id)
        {
            var result1 = await _roleManager.FindByIdAsync(id.ToString()); // return role
            if (result1 is null) return false;
            var result2 = await _roleManager.RoleExistsAsync(roleName); // return bool
            if (result2 == true && result1.Name == roleName)
                return true;
            return false;
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return role;
        }

        public async Task<IReadOnlyList<Role>?> GetRolesListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }
        #endregion
    }
}
