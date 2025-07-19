using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Service.Services.Contract
{
    public interface IProductService
    {
        IQueryable<Product> FilterProductPaginatedQueryable(ProductSortingEnum sortBy, string search);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<string> AddProductAsync(Product product);
        Task<string> EditProductAsync(Product product);
        Task<string> DeleteProductAsync(Product product);
        Task<bool> IsNameExist(string name);
        Task<bool> IsNameExistExcludeSelf(string name, Guid id);
    }
}
