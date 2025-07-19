using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        #region Fields
        private readonly DbSet<RefreshToken> _refreshTokens;
        #endregion

        #region Constructors
        public RefreshTokenRepository(E_CommerceContext dbContext) : base(dbContext)
        {
            _refreshTokens = dbContext.Set<RefreshToken>();
        }
        #endregion

        #region Handle Functions

        #endregion
    }
}
