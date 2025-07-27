using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Service.AuthService.Services.Contract
{
    public interface ICurrentUserService
    {
        public Guid GetUserId();
        public Task<User> GetUserAsync();
        public Task<List<string>> GetCurrentUserRolesAsync();
    }
}
