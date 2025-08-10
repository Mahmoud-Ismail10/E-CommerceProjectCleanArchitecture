using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        private readonly DbSet<Payment> _payments;
        public PaymentRepository(E_CommerceContext dbContext) : base(dbContext)
        {
            _payments = dbContext.Set<Payment>();
        }

        public async Task<Payment?> GetPaymentByTransactionId(string transactionId)
        {
            return await _payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<Payment?> GetPaymentByOrderId(Guid orderId)
        {
            return await _payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}
