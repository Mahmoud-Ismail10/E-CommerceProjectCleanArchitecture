using E_Commerce.Domain.Entities.Identity;

namespace E_Commerce.Service.Services.Contract
{
    public interface IApplicationUserService
    {
        Task<string> AddUserAsync(User user, string password);
    }
}
