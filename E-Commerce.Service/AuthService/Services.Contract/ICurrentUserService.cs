using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Service.AuthService.Services.Contract
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        Guid GetUserId();
        Guid GetCartOwnerId();
        Task<User> GetUserAsync();
        Task<List<string>> GetCurrentUserRolesAsync();
        bool DeleteGuestIdCookie();
    }
}
