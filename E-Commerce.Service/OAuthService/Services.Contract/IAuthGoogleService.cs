using E_Commerce.Domain.Helpers;
using Google.Apis.Auth;

namespace Ecommerce.DataAccess.Services.OAuth
{
    public interface IAuthGoogleService
    {
        Task<(JwtAuthResponse, string)> AuthenticateWithGoogleAsync(string idToken);
        Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
    }
}