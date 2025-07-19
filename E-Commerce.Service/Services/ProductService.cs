using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> AddProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
            return "Success";
        }

        public async Task<string> DeleteProductAsync(Product product)
        {
            var transaction = _productRepository.BeginTransaction();
            try
            {
                await _productRepository.DeleteAsync(product);
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error deleting product: {ex.Message}";
            }
        }

        public async Task<string> EditProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
            return "Success";
        }

        public IQueryable<Product> FilterProductPaginatedQueryable(ProductSortingEnum sortBy, string search)
        {
            var queryable = _productRepository.GetTableNoTracking().Include(p => p.Category).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                queryable = queryable.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
            queryable = sortBy switch
            {
                ProductSortingEnum.NameAsc => queryable.OrderBy(c => c.Name),
                ProductSortingEnum.NameDesc => queryable.OrderByDescending(c => c.Name),
                ProductSortingEnum.PriceAsc => queryable.OrderBy(c => c.Price),
                ProductSortingEnum.PriceDesc => queryable.OrderByDescending(c => c.Price),
                ProductSortingEnum.StockQuantityAsc => queryable.OrderBy(c => c.StockQuantity),
                ProductSortingEnum.StockQuantityDesc => queryable.OrderByDescending(c => c.StockQuantity),
                ProductSortingEnum.CreatedDateAsc => queryable.OrderBy(c => c.CreatedAt),
                ProductSortingEnum.CreatedDateDesc => queryable.OrderByDescending(c => c.CreatedAt),
                ProductSortingEnum.RatingAsc => queryable.OrderBy(c => c.Reviews.Any() ? c.Reviews.Max(r => r.Rating) : 0),
                ProductSortingEnum.RatingDesc => queryable.OrderByDescending(c => c.Reviews.Any() ? c.Reviews.Max(r => r.Rating) : 0),
                _ => queryable.OrderBy(c => c.Name)
            };
            return queryable;

        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetTableNoTracking()
                                              .Where(c => c.Id.Equals(id))
                                              .Include(c => c.Category)
                                              .FirstOrDefaultAsync();
            return product;
        }

        public async Task<bool> IsNameExist(string name)
        {
            var product = await _productRepository.GetTableNoTracking()
                                  .Where(c => c.Name.Equals(name))
                                  .FirstOrDefaultAsync();
            if (product != null) return true;
            return false;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, Guid id)
        {
            var product = await _productRepository.GetTableNoTracking()
                                              .Where(c => c.Name.Equals(name) & !c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            if (product != null) return true;
            return false;
        }
    }
}
