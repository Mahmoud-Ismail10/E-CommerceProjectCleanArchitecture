using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Infrastructure.Bases;

namespace E_Commerce.Infrastructure.Repositories.Contract
{
    public interface IPaymentRepository : IGenericRepositoryAsync<Payment>
    {
        Task<Payment?> GetPaymentByTransactionId(string transactionId);
        Task<Payment?> GetPaymentByOrderId(Guid orderId);
    }
}
