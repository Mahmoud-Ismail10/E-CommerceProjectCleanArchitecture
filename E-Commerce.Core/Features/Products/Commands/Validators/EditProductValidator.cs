using E_Commerce.Core.Features.Products.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Products.Commands.Validators
{
    public class EditProductValidator : AbstractValidator<EditProductCommand>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public EditProductValidator(IProductService productService, ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _productService = productService;
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

            RuleFor(c => c.Price)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(c => c.StockQuantity)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }

        public void ApplyCustomValidationRoles()
        {
            RuleFor(c => c.Name)
                .MustAsync(async (model, name, cancellation) => !await _productService.IsNameExistExcludeSelf(name, model.Id))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.IsExist]);

            RuleFor(c => c.CategoryId)
                .MustAsync(async (key, cancellation) => await _categoryService.IsCategoryIdExist(key))
                .WithMessage(_stringLocalizer[SharedResourcesKeys.IsNotExist]);
        }
    }
}
