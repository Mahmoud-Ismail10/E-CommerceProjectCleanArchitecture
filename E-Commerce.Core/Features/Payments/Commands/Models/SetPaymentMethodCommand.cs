using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Payments.Commands.Models
{
    public record SetPaymentMethodCommand(Guid OrderId, PaymentMethod PaymentMethod) : IRequest<ApiResponse<string>>;
}
