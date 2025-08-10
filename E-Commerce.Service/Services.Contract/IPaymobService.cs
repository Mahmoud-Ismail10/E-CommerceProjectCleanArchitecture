
using X.Paymob.CashIn.Models.Callback;

namespace E_Commerce.Service.Services.Contract
{
    public interface IPaymobService
    {
        string ComputeHmacSHA512(string data, string secret);
        Task<string> CreateOrUpdatePaymentAsync(Guid cartId);
        string GetPaymentIframeUrl(string paymentToken);
        Task<string> ProcessPaymentForOrderAsync(Guid orderId);
        Task<string> ProcessTransactionCallback(CashInCallbackTransaction callback);
        Task<string> UpdateOrderFailed(string paymentIntentId);
        Task<string> UpdateOrderSuccessAsync(string paymentIntentId);
    }
}
