using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface IDeliveryService
    {
        Task<string> AddDeliveryAsync(Delivery delivery);
    }
}
