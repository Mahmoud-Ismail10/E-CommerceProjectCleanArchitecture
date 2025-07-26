using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authentication.Queries.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authentication.Queries.Handlers
{
    public class AuthenticationQueryHandler : ApiResponseHandler,
        IRequestHandler<AuthorizeUserQuery, ApiResponse<string>>,
        IRequestHandler<ConfirmEmailQuery, ApiResponse<string>>,
        IRequestHandler<ConfirmResetPasswordQuery, ApiResponse<string>>
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AuthenticationQueryHandler(IAuthenticationService authenticationService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authenticationService = authenticationService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ValidateToken(request.AccessToken);
            if (result == "NotExpired")
                return Success(result);
            return Unauthorized<string>(_stringLocalizer[SharedResourcesKeys.TokenIsExpired]);
        }

        public async Task<ApiResponse<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var confirmEmailResult = await _authenticationService.ConfirmEmailAsync(request.UserId, request.Code);
            return confirmEmailResult switch
            {
                "UserOrCodeIsNullOrEmpty" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserOrCodeIsNullOrEmpty]),
                "Success" => Success<string>(_stringLocalizer[SharedResourcesKeys.ConfirmEmailDone]),
                _ => BadRequest<string>(confirmEmailResult)
            };
        }

        public async Task<ApiResponse<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
        {
            var confirmResetPasswordResult = await _authenticationService.ConfirmResetPasswordAsync(request.Code, request.Email);
            return confirmResetPasswordResult switch
            {
                "UserNotFound" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNotFound]),
                "Success" => Success(""),
                _ => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode])
            };
        }
        #endregion
    }
}
