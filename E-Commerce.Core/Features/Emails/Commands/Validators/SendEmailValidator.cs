using E_Commerce.Core.Features.Emails.Commands.Models;
using E_Commerce.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Emails.Commands.Validators
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public SendEmailValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.ReturnUrl)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }
        #endregion
    }
}
