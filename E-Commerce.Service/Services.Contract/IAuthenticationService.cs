using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace E_Commerce.Service.Services.Contract
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResponse> GetJWTTokenAsync(User user);
        public Task<JwtAuthResponse> GetRefreshTokenAsync(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        public JwtSecurityToken ReadJwtToken(string accessToken);
        public Task<string> ValidateToken(string accessToken);
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken);
    }
}
