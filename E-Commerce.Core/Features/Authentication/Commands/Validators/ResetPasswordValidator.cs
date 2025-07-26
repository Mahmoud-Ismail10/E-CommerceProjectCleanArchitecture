using E_Commerce.Core.Features.Authentication.Commands.Models;
using E_Commerce.Core.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Authentication.Commands.Validators;
public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    #endregion

    #region Constructors
    public ResetPasswordValidator(IStringLocalizer<SharedResources> stringLocalizer)
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
            .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
            .EmailAddress().WithMessage(_stringLocalizer[SharedResourcesKeys.InvalidFormat]);

        RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .MinimumLength(6).WithMessage(_stringLocalizer[SharedResourcesKeys.MinLengthIs6])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

        RuleFor(c => c.ConfirmPassword)
            .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
            .Equal(c => c.NewPassword).WithMessage(_stringLocalizer[SharedResourcesKeys.PasswordsDoNotMatch]);
    }
    #endregion
}
