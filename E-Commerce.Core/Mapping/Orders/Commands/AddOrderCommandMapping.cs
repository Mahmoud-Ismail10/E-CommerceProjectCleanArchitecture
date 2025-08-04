using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Orders
{
    public partial class OrderProfile
    {
        public void AddOrderCommandMapping()
        {
            CreateMap<AddOrderCommand, Order>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow.ToLocalTime()));
        }
    }
}
