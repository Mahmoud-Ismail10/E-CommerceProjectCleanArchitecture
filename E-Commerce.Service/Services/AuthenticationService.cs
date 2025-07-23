using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        #endregion

        #region Constructors
        public AuthenticationService(UserManager<User> userManager, JwtSettings jwtSettings, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<JwtAuthResponse> GetJWTTokenAsync(User user)
        {

            var (jwtToken, accessToken) = await GenerateJwtToken(user);
            var refreshToken = GetRefreshToken(user.UserName);
            var userRefreshToken = new UserRefreshToken
            {
                // AddedTime is set to default value at Fluent API configuration
                IsUsed = true,
                IsRevoked = false,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                JwtId = jwtToken.Id,
                RefreshToken = refreshToken.TokenString,
                Token = accessToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken);

            var response = new JwtAuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return response;
        }

        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(User user)
        {
            var claims = await GetClaims(user);
            var jwtToken = new JwtSecurityToken(_jwtSettings.Issuer,
                                                   _jwtSettings.Audience,
                                                   claims,
                                                   expires: DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate),
                                                   signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),
                new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }

        private RefreshToken GetRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                UserName = userName,
                TokenString = GenerateRefreshToken(),
                ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
            };
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<JwtAuthResponse> GetRefreshTokenAsync(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {
            var (jwtSecurityToken, newToken) = await GenerateJwtToken(user);
            var response = new JwtAuthResponse();
            response.AccessToken = newToken;

            var refreshTokenResult = new RefreshToken();
            refreshTokenResult.UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName)).Value;
            refreshTokenResult.TokenString = refreshToken;
            refreshTokenResult.ExpireAt = (DateTime)expiryDate;

            response.RefreshToken = refreshTokenResult;
            return response;
        }

        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException(nameof(accessToken), "Access token cannot be null or empty.");
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(accessToken))
                throw new ArgumentException("Invalid JWT token.", nameof(accessToken));
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }

        public async Task<string> ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };
            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                if (validator == null)
                    return "Invalid JWT token.";

                return "Not Expired.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
                return ("AlgorithmIsWrong", null);
            if (jwtToken.ValidTo > DateTime.UtcNow)
                return ("TokenIsNotExpired", null);

            //Get User
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;
            var userRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                                                .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                                                                    x.RefreshToken == refreshToken &&
                                                                                    x.UserId == Guid.Parse(userId));
            //Validate User Refresh Token
            if (userRefreshToken == null)
                return ("RefreshTokenIsNotFound", null);

            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _refreshTokenRepository.UpdateAsync(userRefreshToken);
                return ("RefreshTokenIsExpired", null);
            }
            var expirydate = userRefreshToken.ExpiryDate;
            return (userId, expirydate);
        }

        #endregion
    }
}
