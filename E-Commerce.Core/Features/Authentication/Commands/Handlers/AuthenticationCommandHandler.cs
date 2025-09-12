using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authentication.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.Services.Contract;
using Ecommerce.DataAccess.Services.OAuth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationCommandHandler : ApiResponseHandler,
        IRequestHandler<SignInCommand, ApiResponse<JwtAuthResponse>>,
        IRequestHandler<RefreshTokenCommand, ApiResponse<JwtAuthResponse>>,
        IRequestHandler<SendResetPasswordCommand, ApiResponse<string>>,
        IRequestHandler<ResetPasswordCommand, ApiResponse<string>>,
        IRequestHandler<GoogleLoginCommand, ApiResponse<JwtAuthResponse>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthGoogleService _authGoogleService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AuthenticationCommandHandler(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthenticationService authenticationService,
            IAuthGoogleService authGoogleService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _authGoogleService = authGoogleService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null) return BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.UserNameIsNotExist]);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!user.EmailConfirmed) return BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.EmailIsNotConfirmed]);
            if (!signInResult.Succeeded) return BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.InvalidPassword]);

            var result = await _authenticationService.GetJWTTokenAsync(user);
            return Success(result);
        }

        public async Task<ApiResponse<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var jwtToken = _authenticationService.ReadJwtToken(request.AccessToken);
            var userIdAndExpireDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
            switch (userIdAndExpireDate)
            {
                case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.AlgorithmIsWrong]);
                case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.TokenIsNotExpired]);
                case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
                case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsExpired]);
            }
            var (userId, expiryDate) = userIdAndExpireDate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound<JwtAuthResponse>();
            }
            var result = await _authenticationService.GetRefreshTokenAsync(user, jwtToken, expiryDate, request.RefreshToken);
            return Success(result);
        }

        public async Task<ApiResponse<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetPasswordResult = await _authenticationService.SendResetPasswordCodeAsync(request.Email);
            return resetPasswordResult switch
            {
                "Success" => Success(""),
                "UserNotFound" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNotFound]),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainLater])
            };
        }

        public async Task<ApiResponse<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetPasswordResult = await _authenticationService.ResetPasswordAsync(request.Email, request.NewPassword);
            return resetPasswordResult switch
            {
                "Success" => Success(""),
                "UserNotFound" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNotFound]),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode])
            };
        }

        public async Task<ApiResponse<JwtAuthResponse>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var (response, message) = await _authGoogleService.AuthenticateWithGoogleAsync(request.IdToken);
            return message switch
            {
                "Success" => Success(response),
                "InvalidGoogleToken" => BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.InvalidGoogleToken]),
                "FailedToAddNewRoles" => BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]),
                "FailedToAddNewClaims" => BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]),
                "GoogleAuthenticationFailed" => BadRequest<JwtAuthResponse>(_stringLocalizer[SharedResourcesKeys.GoogleAuthenticationFailed]),
                _ => BadRequest<JwtAuthResponse>(message),
            };
        }
        #endregion
    }
}
