using E_Commerce.Core.Features.Reviews.Queries.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Reviews.Queries.Validators
{
    public class GetReviewPaginatedListValidator : AbstractValidator<GetReviewPaginatedListQuery>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public GetReviewPaginatedListValidator(IProductService productService, ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);
        }
        #endregion
    }
}
