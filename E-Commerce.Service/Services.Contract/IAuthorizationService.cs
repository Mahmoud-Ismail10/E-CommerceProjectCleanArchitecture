using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Requests;
using E_Commerce.Domain.Responses;

namespace E_Commerce.Service.Services.Contract
{
    public interface IAuthorizationService
    {
        Task<string> AddRoleAsync(string roleName);
        Task<string> EditRoleAsync(string roleName, Guid id);
        Task<string> DeleteRoleAsync(Guid id);
        Task<Role?> GetRoleByIdAsync(Guid id);
        Task<IReadOnlyList<Role>?> GetRolesListAsync();
        Task<bool> IsRoleExistByName(string roleName);
        Task<bool> IsRoleExistById(Guid id);
        Task<bool> IsRoleExistExcludeSelf(string roleName, Guid id);
        Task<ManageUserRolesResponse> ManageUserRolesData(User user);
        Task<string> UpdateUserRoles(UpdateUserRolesRequest request);
        Task<ManageUserClaimsResponse> ManageUserClaimsData(User user);
        Task<string> UpdateUserClaims(UpdateUserClaimsRequest request);
    }
}
