using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.ApplicationUser.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.ApplicationUser.Commands.Handlers
{
    internal class UserCommandHandler : ApiResponseHandler,
        IRequestHandler<AddCustomerCommand, ApiResponse<string>>,
        IRequestHandler<ChangeUserPasswordCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public UserCommandHandler(UserManager<User> userManager, IMapper mapper, IApplicationUserService applicationUserService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _mapper = mapper;
            _applicationUserService = applicationUserService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user is null) return NotFound<string>();

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
                return BadRequest<string>(changePasswordResult.Errors.FirstOrDefault()?.Description);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.PasswordChangedSuccessfully]);
        }

        public async Task<ApiResponse<string>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var identityUser = _mapper.Map<Customer>(request);
            var createResult = await _applicationUserService.AddUserAsync(identityUser, request.Password);
            return createResult switch
            {
                "EmailIsExist" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsExist]),
                "UserNameIsExist" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]),
                "FailedToAddNewRoles" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]),
                "FailedToAddNewClaims" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]),
                "SendEmailFailed" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SendEmailFailed]),
                "Failed" => BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryToRegisterAgain]),
                "Success" => Created(""),
                _ => BadRequest<string>(createResult)
            };

            #region Old Style of Switch Case
            //switch (createResult)
            //{
            //    case "EmailIsExist": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EmailIsExist]);
            //    case "UserNameIsExist": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
            //    case "FailedToAddNewRoles": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]);
            //    case "FailedToAddNewClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]);
            //    case "SendEmailFailed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SendEmailFailed]);
            //    case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryToRegisterAgain]);
            //    case "Success": return Created("");
            //    default: return BadRequest<string>(createResult);
            //} 
            #endregion
        }
        #endregion
    }
}
