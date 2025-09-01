namespace E_Commerce.Core.Features.Orders.Commands.Responses
{
    public record PaymentProcessResponse(
        Guid OrderId,
        string PaymentUrl,
        string PaymentToken);
}
