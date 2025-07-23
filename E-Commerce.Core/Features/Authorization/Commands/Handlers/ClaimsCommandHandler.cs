using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authorization.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authorization.Commands.Handlers
{
    public class ClaimsCommandHandler : ApiResponseHandler,
        IRequestHandler<UpdateUserClaimsCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ClaimsCommandHandler(IAuthorizationService authorizationService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserClaims(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>();
                case "FailedToRemoveOldClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToRemoveOldClaims]);
                case "FailedToAddNewClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]);
                case "FailedToUpdateUserClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateUserClaims]);
            }
            return Edit("");
        }
        #endregion
    }
}
