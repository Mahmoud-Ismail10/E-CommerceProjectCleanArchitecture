using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Infrastructure.Bases;

namespace E_Commerce.Infrastructure.Repositories.Contract
{
    public interface IPaymentRepository : IGenericRepositoryAsync<Payment>
    {
    }
}
