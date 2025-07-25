using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Service.AuthService.Services.Contract
{
    public interface ICurrentUserService
    {
        public Task<User> GetUserAsync();
        public int GetUserId();
        public Task<List<string>> GetCurrentUserRolesAsync();
    }
}
