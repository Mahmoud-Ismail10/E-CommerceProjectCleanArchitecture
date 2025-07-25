using E_Commerce.Core.Features.ApplicationUser.Commands.Models;
using E_Commerce.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.ApplicationUser.Commands.Validators
{
    public class AddCustomerValidator : AbstractValidator<AddCustomerCommand>
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public AddCustomerValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }

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
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MinimumLength(6).WithMessage(_stringLocalizer[SharedResourcesKeys.MinLengthIs6]);
            RuleFor(c => c.ConfirmPassword)
                .Equal(c => c.Password).WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordsDoNotMatch]);
        }
    }
}
