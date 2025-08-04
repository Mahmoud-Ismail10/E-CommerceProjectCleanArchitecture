using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Infrastructure.Bases;
using E_Commerce.Infrastructure.Repositories.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {
        private readonly DbSet<Product> _products;

        public ProductRepository(E_CommerceContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Product>();
        }

        public async Task<Dictionary<Guid, string?>> GetProductsByIdsAsync(List<Guid> productIds)
        {
            var products = await _products.Where(p => productIds.Contains(p.Id))
                                            .ToDictionaryAsync(p => p.Id, p => p.Name);
            return products;
        }
    }
}
