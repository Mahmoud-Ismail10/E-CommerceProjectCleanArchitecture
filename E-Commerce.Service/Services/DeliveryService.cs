using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class DeliveryService : IDeliveryService
    {
        #region Fields
        private readonly IDeliveryRepository _deliveryRepository;

        #endregion

        #region Constructors
        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddDeliveryAsync(Delivery delivery)
        {
            try
            {
                await _deliveryRepository.AddAsync(delivery);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error adding delivery : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "FailedInAdd";
            }
        }
        #endregion
    }
}
