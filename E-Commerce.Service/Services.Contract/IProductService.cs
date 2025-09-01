using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using Microsoft.AspNetCore.Http;

namespace E_Commerce.Service.Services.Contract
{
    public interface IProductService
    {
        IQueryable<Product> FilterProductPaginatedQueryable(ProductSortingEnum sortBy, string search);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Dictionary<Guid, string?>> GetProductsByIdsAsync(List<Guid> productIds);
        Task<string> AddProductAsync(Product product, IFormFile file);
        Task<string> EditProductAsync(Product product);
        Task<string> DeleteProductAsync(Product product);
        Task<bool> IsNameExist(string name);
        Task<bool> IsNameExistExcludeSelf(string name, Guid id);
        Task<string> DiscountQuantityFromStock(List<OrderItem> orderItems);
    }
}
