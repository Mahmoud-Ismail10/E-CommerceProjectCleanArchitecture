using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.Services.Contract;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Security.Claims;

namespace Ecommerce.DataAccess.Services.OAuth
{
    public class AuthGoogleService : IAuthGoogleService
    {
        private readonly UserManager<User> _userManager;
        private readonly GoogleAuthSettings _settings;
        private readonly IAuthenticationService _authenticationService;

        public AuthGoogleService(UserManager<User> userManager,
            GoogleAuthSettings settings,
            IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _settings = settings;
            _authenticationService = authenticationService;
        }

        public async Task<(JwtAuthResponse, string)> AuthenticateWithGoogleAsync(string idToken)
        {
            try
            {
                // Validate the ID token and retrieve the payload
                var payload = await ValidateGoogleTokenAsync(idToken);

                if (payload == null)
                    return (null!, "InvalidGoogleToken");

                // Check if the user already exists by email
                var user = await _userManager.FindByEmailAsync(payload.Email);

                // If user doesn't exist, create a new user
                if (user == null)
                {
                    user = new Customer
                    {
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        Gender = Gender.Unspecified,
                        UserName = payload.Email.Split('@')[0],
                        Email = payload.Email,
                        EmailConfirmed = payload.EmailVerified,
                        PhoneNumber = "N/A",
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return (null!, $"Failed to create customer: {errors}");
                    }

                    //Add default role "Customer"
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "Customer");
                    if (!addToRoleResult.Succeeded)
                        return (null!, "FailedToAddNewRoles");

                    //Add default customer policies
                    var claims = new List<Claim>
                    {
                        new Claim("Edit Customer", "True"),
                        new Claim("Get Customer", "True")
                    };
                    var addDefaultClaimsResult = await _userManager.AddClaimsAsync(user, claims);
                    if (!addDefaultClaimsResult.Succeeded)
                        return (null!, "FailedToAddNewClaims");
                }
                else
                {
                    // If user exists but email is not confirmed, confirm it if Google says it's verified
                    if (payload.EmailVerified && !user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                    }
                }
                var response = await _authenticationService.GetJWTTokenAsync(user);
                return (response, "Success");
            }
            catch (Exception ex)
            {
                Log.Error("Google authentication failed: {Message}", ex.InnerException?.Message ?? ex.Message);
                return (null!, "GoogleAuthenticationFailed");
            }
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _settings.ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);

                if (payload == null || string.IsNullOrEmpty(payload.Email))
                    throw new UnauthorizedAccessException("Invalid Google Token: Payload is null or missing email");

                return payload;
            }
            catch (InvalidJwtException ex)
            {
                Log.Error("Invalid Google Token: " + ex.InnerException?.Message ?? ex.Message);
                throw new UnauthorizedAccessException("Invalid Google Token: " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to validate Google Token: {Message}" + ex.InnerException?.Message ?? ex.Message);
                throw new Exception("Failed to validate Google Token: {Message}" + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}