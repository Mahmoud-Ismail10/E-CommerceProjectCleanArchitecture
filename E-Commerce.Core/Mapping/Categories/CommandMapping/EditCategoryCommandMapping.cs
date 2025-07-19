using E_Commerce.Core.Features.Categories.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Categories
{
    public partial class CategoryProfile
    {
        public void EditCategoryCommandMapping()
        {
            CreateMap<EditCategoryCommand, Category>();
        }
    }
}
