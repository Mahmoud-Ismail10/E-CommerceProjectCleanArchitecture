using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authorization.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authorization.Commands.Handlers
{
    public class RoleCommandHandler : ApiResponseHandler,
        IRequestHandler<AddRoleCommand, ApiResponse<string>>,
        IRequestHandler<EditRoleCommand, ApiResponse<string>>,
        IRequestHandler<DeleteRoleCommand, ApiResponse<string>>,
        IRequestHandler<UpdateUserRolesCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public RoleCommandHandler(IAuthorizationService authorizationService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.AddRoleAsync(request.RoleName);
            if (result == "Success") return Created("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.EditRoleAsync(request.RoleName, request.RoleId);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "Success") return Edit("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeleteRoleAsync(request.RoleId);
            if (result == "NotFound") return NotFound<string>();
            else if (result == "Success") return Deleted<string>();
            else if (result == "Used") return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RoleIsUsed]);
            return BadRequest<string>(result);
        }

        public async Task<ApiResponse<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserRoles(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>();
                case "FailedToRemoveOldRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToRemoveOldRoles]);
                case "FailedToAddNewRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]);
                case "FailedToUpdateUserRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateUserRoles]);
            }
            return Edit("");
        }
        #endregion
    }
}
