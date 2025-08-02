using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public ProductService(IProductRepository productRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddProductAsync(Product product, IFormFile file)
        {
            var context = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = context.Scheme + "://" + context.Host;
            var imageUrl = await _fileService.UploadImageAsync("Products", file);
            switch (imageUrl)
            {
                case "FailedToUploadImage": return "FailedToUploadImage";
                case "NoImage": return "NoImage";
            }
            product.ImageURL = baseUrl + imageUrl;
            try
            {
                await _productRepository.AddAsync(product);
                return "Success";
            }
            catch (Exception)
            {
                Log.Error("Error adding product", product);
                return "FailedInAdd";
            }
        }

        public async Task<string> DeleteProductAsync(Product product)
        {
            var transaction = await _productRepository.BeginTransactionAsync();
            try
            {
                await _productRepository.DeleteAsync(product);
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error("Error deleting product {ProductName}: {ErrorMessage}", product.Name, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
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
                queryable = queryable.Where(c => c.Name!.Contains(search) || c.Description!.Contains(search));
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

        public async Task<Product?> GetProductByIdAsync(Guid id)
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
                                  .Where(c => c.Name!.Equals(name))
                                  .FirstOrDefaultAsync();
            if (product != null) return true;
            return false;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, Guid id)
        {
            var product = await _productRepository.GetTableNoTracking()
                                              .Where(c => c.Name!.Equals(name) & !c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            if (product != null) return true;
            return false;
        }
        #endregion
    }
}
