using E_Commerce.Core.Features.Authorization.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authorization.Commands.Validators
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public DeleteRoleValidator(IAuthorizationService authorizationService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _authorizationService = authorizationService;
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
            //ApplyCustomValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }

        //public void ApplyCustomValidationRoles()
        //{
        //    RuleFor(c => c.RoleId)
        //        .MustAsync(async (id, cancellation) => await _authorizationService.IsRoleExistById(id))
        //        .WithMessage(_stringLocalizer[SharedResourcesKeys.IsNotExist]);
        //}
        #endregion
    }
}