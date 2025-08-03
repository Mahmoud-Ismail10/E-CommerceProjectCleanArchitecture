using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class PaymentService : IPaymentService
    {
        #region Fields
        private readonly IPaymentRepository _paymentRepository;

        #endregion

        #region Constructors
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddPaymentAsync(Payment payment)
        {
            try
            {
                await _paymentRepository.AddAsync(payment);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error adding payment : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "FailedInAdd";
            }
        }
        #endregion
    }
}
