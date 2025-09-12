using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace E_Commerce.Service.Services.Contract
{
    public interface IAuthenticationService
    {
        Task<JwtAuthResponse> GetJWTTokenAsync(User user);
        Task<JwtAuthResponse> GetRefreshTokenAsync(User user, JwtSecurityToken jwtToken, DateTimeOffset? expiryDate, string refreshToken);
        JwtSecurityToken ReadJwtToken(string accessToken);
        Task<string> ValidateToken(string accessToken);
        Task<(string, DateTimeOffset?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        Task<string> ConfirmEmailAsync(Guid? userId, string? code);
        Task<string> SendResetPasswordCodeAsync(string email);
        Task<string> ConfirmResetPasswordAsync(string code, string email);
        Task<string> ResetPasswordAsync(string email, string newPassword);
        RefreshToken GetRefreshToken(string userName);
    }
}
