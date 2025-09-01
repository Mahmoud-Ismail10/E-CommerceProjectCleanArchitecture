using E_Commerce.Core.Features.Payments.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Enums;
using E_Commerce.Service.Services.Contract;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Payments.Commands.Validators
{
    public class SetPaymentMethodValidator : AbstractValidator<SetPaymentMethodCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IOrderService _orderService;
        #endregion

        #region Constructors
        public SetPaymentMethodValidator(IStringLocalizer<SharedResources> stringLocalizer, IOrderService orderService)
        {
            _stringLocalizer = stringLocalizer;
            _orderService = orderService;
            ApplyValidationRoles();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationRoles()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c.PaymentMethod)
                .NotEmpty().WithMessage(_stringLocalizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[SharedResourcesKeys.Required]);

            RuleFor(c => c).CustomAsync(async (command, context, cancellation) =>
            {
                var order = await _orderService.GetOrderByIdAsync(command.OrderId);
                if (order == null)
                {
                    context.AddFailure(nameof(command.OrderId), _stringLocalizer[SharedResourcesKeys.IsNotExist]);
                    return;
                }

                if (!IsValidCombination(command.PaymentMethod, order.Delivery!.DeliveryMethod))
                    context.AddFailure(nameof(command.PaymentMethod), _stringLocalizer[SharedResourcesKeys.InvalidCombination]);
            });
        }

        private bool IsValidCombination(PaymentMethod? paymentMethod, DeliveryMethod? deliveryMethod)
        {
            return (paymentMethod, deliveryMethod) switch
            {
                (PaymentMethod.CashOnDelivery, DeliveryMethod.PickupFromBranch) => false,
                (PaymentMethod.CashAtBranch, DeliveryMethod.Standard) => false,
                (PaymentMethod.CashAtBranch, DeliveryMethod.Express) => false,
                (PaymentMethod.CashAtBranch, DeliveryMethod.SameDay) => false,
                (PaymentMethod.CashAtBranch, DeliveryMethod.Scheduled) => false,
                _ => true
            };
        }
        #endregion
    }
}