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
    }
}
