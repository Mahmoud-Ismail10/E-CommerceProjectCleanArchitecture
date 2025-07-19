using E_Commerce.Core.Features.Categories.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Categories.Commands.Validators
{
    public class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public EditCategoryValidator(ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _categoryService = categoryService;
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
            ApplyCustomValidationRoles();
        }

        public void ApplyValidationRoles()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs100]);
            RuleFor(c => c.Description)
                .MaximumLength(300).WithMessage(_stringLocalizer[SharedResourcesKeys.MaxLengthIs300]);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c.Name)
                .MustAsync(async (model, name, cancellation) => !await _categoryService.IsNameExistExcludeSelf(name, model.Id))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.IsExist]);
        }
    }
}
