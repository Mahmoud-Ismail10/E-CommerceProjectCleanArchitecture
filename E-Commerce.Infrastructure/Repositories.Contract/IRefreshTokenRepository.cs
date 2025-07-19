using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Infrastructure.Infrastructure.Bases;

namespace E_Commerce.Infrastructure.Repositories.Contract
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {
    }
}
