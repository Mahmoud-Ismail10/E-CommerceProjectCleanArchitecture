using E_Commerce.Core.Features.Payments.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Payments.Queries.Models
{
    public record PaymobCallbackQuery(
    string AmountCents,
    string CreatedAt,
    string Currency,
    string ErrorOccured,
    string HasParentTransaction,
    string Id,
    string IntegrationId,
    string Is3dSecure,
    string IsAuth,
    string IsCapture,
    string IsRefunded,
    string IsStandalonePayment,
    string IsVoided,
    string Order,
    string Owner,
    string Pending,
    string SourceDataPan,
    string SourceDataSubType,
    string SourceDataType,
    string Success,
    string MerchantOrderId,
    string Hmac) : IRequest<PaymobCallbackResponse>;
}
