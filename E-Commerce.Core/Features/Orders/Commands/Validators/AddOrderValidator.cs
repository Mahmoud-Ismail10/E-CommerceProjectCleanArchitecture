namespace E_Commerce.Core.Features.Orders.Commands.Validators
{
    //public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    //{
    //    #region Fields
    //    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    //    #endregion

    //    #region Constructors
    //    public AddOrderValidator(IStringLocalizer<SharedResources> stringLocalizer)
    //    {
    //        _stringLocalizer = stringLocalizer;
    //        ApplyValidationRoles();
    //    }
    //    #endregion

    //    #region Handle Functions
    //    public void ApplyValidationRoles()
    //    {
    //        RuleFor(c => c.PaymentMethod)
    //            .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
    //            .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

    //        RuleFor(c => c.DeliveryMethod)
    //            .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
    //            .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

    //        RuleFor(c => c.ShippingAddressId)
    //            .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
    //            .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required])
    //            .When(c => c.DeliveryMethod != DeliveryMethod.PickupFromBranch)
    //                .WithMessage(_stringLocalizer[SharedResourcesKeys.CannotSelectShippingAddress]);

    //        //RuleFor(c => c)
    //        //    .Must(c => IsValidCombination(c.PaymentMethod, c.DeliveryMethod))
    //        //    .WithMessage(_stringLocalizer[SharedResourcesKeys.InvalidCombination]);
    //    }

    //    //private bool IsValidCombination(PaymentMethod? paymentMethod, DeliveryMethod? deliveryMethod)
    //    //{
    //    //    return (paymentMethod, deliveryMethod) switch
    //    //    {
    //    //        (PaymentMethod.CashOnDelivery, DeliveryMethod.PickupFromBranch) => false,
    //    //        (PaymentMethod.CashAtBranch, DeliveryMethod.Standard) => false,
    //    //        (PaymentMethod.CashAtBranch, DeliveryMethod.Express) => false,
    //    //        (PaymentMethod.CashAtBranch, DeliveryMethod.SameDay) => false,
    //    //        (PaymentMethod.CashAtBranch, DeliveryMethod.Scheduled) => false,
    //    //        _ => true
    //    //    };
    //    //}
    //    #endregion
    //}
}
