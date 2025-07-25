using E_Commerce.Core.Features.Customers.Commands.Models;
using E_Commerce.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Customers.Commands.Validators
{
    public class EditCustomerValidator : AbstractValidator<EditCustomerCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public EditCustomerValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(50).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(50).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(_stringLocalizer[SharedResourcesKeys.InvalidFormat]);
        }
        #endregion
    }
}
