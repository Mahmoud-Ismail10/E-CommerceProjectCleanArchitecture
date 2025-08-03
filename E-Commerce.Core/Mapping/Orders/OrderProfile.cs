using AutoMapper;

namespace E_Commerce.Core.Mapping.Orders
{
    public partial class OrderProfile : Profile
    {
        public OrderProfile()
        {
            GetOrderByIdQueryMapping();
            AddOrderCommandMapping();
        }
    }
}
