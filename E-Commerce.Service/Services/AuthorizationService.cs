using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using E_Commerce.Domain.Requests;
using E_Commerce.Domain.Responses;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Service.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly E_CommerceContext _dbContext;
        #endregion

        #region Constructors
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager, E_CommerceContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
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

        public async Task<ManageUserRolesResponse> ManageUserRolesData(User user)
        {
            var response = new ManageUserRolesResponse();
            var roles = await _roleManager.Roles.ToListAsync();
            response.UserId = user.Id;
            response.UserRoles = new List<UserRoles>();

            foreach (var role in roles)
            {
                // We don't use .Contant() because it returns invalid data (If we have SuperAdmin, Admin Roles)
                var hasRole = await _userManager.IsInRoleAsync(user, role.Name);
                response.UserRoles.Add(new UserRoles
                {
                    Id = role.Id,
                    Name = role.Name,
                    HasRole = hasRole
                });
            }
            return response;
        }

        public async Task<ManageUserClaimsResponse> ManageUserClaimsData(User user)
        {
            var response = new ManageUserClaimsResponse();
            var userClaimList = new List<UserClaims>();
            var userClaims = await _userManager.GetClaimsAsync(user);
            response.UserId = user.Id;
            response.UserClaims = ClaimsStore.claims.Select(claim => new UserClaims
            {
                Type = claim.Type,
                Value = userClaims.Any(x => x.Type == claim.Type)
            }).ToList();
            return response;
        }

        public async Task<string> UpdateUserRoles(UpdateUserRolesRequest request)
        {
            var transact = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                    return "UserIsNull";
                var userRoles = await _userManager.GetRolesAsync(user);

                //Delete Old Roles
                var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded)
                    return "FailedToRemoveOldRoles";
                var selectedRoles = request.UserRoles.Where(x => x.HasRole == true).Select(x => x.Name);

                //Add the Roles HasRole = True
                var addRolesresult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addRolesresult.Succeeded)
                    return "FailedToAddNewRoles";
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "FailedToUpdateUserRoles";
            }
        }

        public async Task<string> UpdateUserClaims(UpdateUserClaimsRequest request)
        {
            var transact = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                    return "UserIsNull";
                var userClaims = await _userManager.GetClaimsAsync(user);

                //Delete Old Claims
                var removeResult = await _userManager.RemoveClaimsAsync(user, userClaims);
                if (!removeResult.Succeeded)
                    return "FailedToRemoveOldClaims";
                var selectedClaims = request.UserClaims.Where(x => x.Value == true).Select(x => new Claim(x.Type, x.Value.ToString()));

                //Add the Claims Value = True
                var addClaimsresult = await _userManager.AddClaimsAsync(user, selectedClaims);
                if (!addClaimsresult.Succeeded)
                    return "FailedToAddNewClaims";
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "FailedToUpdateUserClaims";
            }
        }
        #endregion
    }
}
