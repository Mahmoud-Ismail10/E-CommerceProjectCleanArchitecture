using E_Commerce.Core.Features.Employees.Commands.Models;
using E_Commerce.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Employees.Commands.Validators
{
    public class AddEmployeeValidator : AbstractValidator<AddEmployeeCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AddEmployeeValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }
        #endregion

        #region Functions
        public void ApplyValidationRoles()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(50).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(e => e.UserName)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(50).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .EmailAddress().WithMessage(_stringLocalizer[SharedResourcesKeys.InvalidFormat]);

            RuleFor(e => e.Password)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MinimumLength(6).WithMessage(_stringLocalizer[SharedResourcesKeys.MinLengthIs6]);

            RuleFor(e => e.ConfirmPassword)
                .Equal(e => e.Password).WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordsDoNotMatch]);

            RuleFor(e => e.Position)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(e => e.Salary)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .GreaterThan(0).WithMessage(_stringLocalizer[SharedResourcesKeys.GreaterThanZero]);

            RuleFor(e => e.Address)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(200).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs200]);
        }
        #endregion
    }
}
