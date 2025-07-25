using E_Commerce.Core.Features.Authentication.Queries.Models;
using E_Commerce.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authentication.Queries.Validators
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailQuery>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ConfirmEmailValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.Code)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }
        #endregion
    }
}
