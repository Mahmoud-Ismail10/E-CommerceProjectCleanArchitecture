using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.ApplicationUser.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.ApplicationUser.Commands.Handlers
{
    internal class UserCommandHandler : ApiResponseHandler,
        IRequestHandler<ChangeUserPasswordCommand, ApiResponse<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public UserCommandHandler(UserManager<User> userManager, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<ApiResponse<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user is null) return NotFound<string>();

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!changePasswordResult.Succeeded)
                return BadRequest<string>(changePasswordResult.Errors.FirstOrDefault()?.Description);
            return Success<string>(_stringLocalizer[SharedResourcesKeys.PasswordChangedSuccessfully]);
        }
    }
}
