using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace E_Commerce.Service.Services.Contract
{
    public interface IAuthenticationService
    {
        Task<JwtAuthResponse> GetJWTTokenAsync(User user);
        Task<JwtAuthResponse> GetRefreshTokenAsync(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        JwtSecurityToken ReadJwtToken(string accessToken);
        Task<string> ValidateToken(string accessToken);
        Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken);
        Task<string> ConfirmEmailAsync(Guid? userId, string? code);
    }
}
