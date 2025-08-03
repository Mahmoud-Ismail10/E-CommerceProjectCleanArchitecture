using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface IPaymentService
    {
        Task<string> AddPaymentAsync(Payment payment);
    }
}
