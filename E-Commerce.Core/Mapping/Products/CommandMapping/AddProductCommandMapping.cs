using E_Commerce.Core.Features.Products.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Products
{
    public partial class ProductProfile
    {
        public void AddProductCommandMapping()
        {
            CreateMap<AddProductCommand, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow.ToLocalTime()))
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore());
        }
    }
}
