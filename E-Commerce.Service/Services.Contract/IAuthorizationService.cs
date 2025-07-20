using E_Commerce.Domain.Entities.Identity;

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
    }
}
