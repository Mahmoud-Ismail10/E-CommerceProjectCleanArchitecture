using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Carts.Commands.Validators
{
    internal class RemoveFromCartValidator : AbstractValidator<RemoveFromCartCommand>
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public RemoveFromCartValidator(ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }

        public void ApplyValidationRoles()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }
    }
}

