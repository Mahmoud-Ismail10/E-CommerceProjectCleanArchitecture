using E_Commerce.Core.Features.Orders.Queries.Responses;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Orders
{
    public partial class OrderProfile
    {
        public void GetOrderByIdQueryMapping()
        {
            CreateMap<Order, GetSingleOrderResponse>()
             .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status))
             .ForMember(dest => dest.CustomerName,
                        opt => opt.MapFrom(src => src.Customer != null ? $"{src.Customer.FirstName} {src.Customer.LastName}" : null))
             .ForMember(dest => dest.ShippingAddress,
                        opt => opt.MapFrom(src => src.ShippingAddress != null ? $"{src.ShippingAddress.State}, {src.ShippingAddress.City}, {src.ShippingAddress.Street}" : null))
             .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Status))
             .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment!.PaymentMethod))
             .ForMember(dest => dest.DeliveryCost,
                         opt => opt.MapFrom(src => src.Delivery != null ? src.Delivery.Cost : 0))
             .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.Delivery!.DeliveryMethod))
             .ForMember(dest => dest.OrderItems, opt => opt.Ignore());
        }
    }
}
