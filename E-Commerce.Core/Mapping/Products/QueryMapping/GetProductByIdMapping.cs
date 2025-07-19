using E_Commerce.Core.Features.Products.Queries.Responses;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Products
{
    public partial class ProductProfile
    {
        public void GetProductByIdMapping()
        {
            CreateMap<Product, GetSingleProductResponse>()
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.Reviews, opt => opt.Ignore());
        }
    }
}
