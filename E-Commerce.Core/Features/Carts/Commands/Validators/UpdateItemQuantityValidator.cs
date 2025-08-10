using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Carts.Commands.Validators
{
    internal class UpdateItemQuantityValidator : AbstractValidator<UpdateItemQuantityCommand>
    {
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public UpdateItemQuantityValidator(ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }

        public void ApplyValidationRoles()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.Quantity)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .GreaterThan(0).WithMessage(_stringLocalizer[SharedResourcesKeys.GreaterThanZero]);
        }
    }
}
