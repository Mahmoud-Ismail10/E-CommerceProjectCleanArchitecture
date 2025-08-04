using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Orders.Commands.Validators
{
    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public AddOrderValidator(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            //RuleForEach(c => c.OrderItemResults).ChildRules(item =>
            //{
            //    item.RuleFor(i => i.ProductId)
            //        .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
            //        .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            //    item.RuleFor(i => i.Quantity)
            //        .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
            //        .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
            //        .GreaterThan(0).WithMessage(_stringLocalizer[SharedResourcesKeys.GreaterThanZero]);
            //});

            RuleFor(c => c.CartId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.PaymentMethod)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.DeliveryMethod)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.ShippingAddressId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
                .When(c => c.DeliveryMethod != DeliveryMethod.Pickup);
        }
        #endregion
    }
}
