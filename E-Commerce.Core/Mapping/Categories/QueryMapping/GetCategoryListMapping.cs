using E_Commerce.Core.Features.Categories.Queries.Response;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Mapping.Categories
{
    public partial class CategoryProfile
    {
        public void GetCategoryListMapping()
        {
            CreateMap<Category, GetCategoryListResponse>();
        }
    }
}
