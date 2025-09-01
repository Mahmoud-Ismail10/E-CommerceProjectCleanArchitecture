
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Responses;

namespace E_Commerce.Service.Services.Contract
{
    public interface IPaymobService
    {
        string ComputeHmacSHA512(string data, string secret);
        string GetPaymentIframeUrl(string paymentToken);
        //Task<string> CreateOrUpdatePaymentAsync(Guid cartId);
        Task<(Order?, string)> ProcessPaymentForOrderAsync(Order order);
        Task<string> ProcessTransactionCallbackAsync(CustomCashInCallbackTransaction callback, Guid orderId);
    }
}
