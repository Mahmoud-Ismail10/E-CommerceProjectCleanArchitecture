using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<string> AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return "Success";
        }

        public async Task<string> DeleteCategoryAsync(Category category)
        {
            var transaction = _categoryRepository.BeginTransaction();
            try
            {
                /// Remove the category
                await _categoryRepository.DeleteAsync(category);
                /// Commit the transaction
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                /// Rollback the transaction in case of error
                await transaction.RollbackAsync();
                return $"Error deleting category: {ex.Message}";
            }
        }

        public async Task<string> EditCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            return "Success";
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetTableNoTracking()
                                              .Where(c => c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            return category;
        }

        public async Task<IReadOnlyList<Category>> GetCategoryListAsync()
        {
            //return await _categoryRepository.GetAllCategoriesAsync();
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<bool> IsNameExist(string name)
        {
            var category = _categoryRepository.GetTableNoTracking()
                                              .Where(c => c.Name.Equals(name))
                                              .FirstOrDefault();
            if (category != null) return true;
            return false;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, Guid id)
        {
            var category = await _categoryRepository.GetTableNoTracking()
                                              .Where(c => c.Name.Equals(name) & !c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            if (category != null) return true;
            return false;
        }

        public IQueryable<Category> FilterCategoryPaginatedQueryable(CategorySortingEnum sortBy, string search)
        {
            var queryable = _categoryRepository.GetTableNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
                queryable = queryable.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
            queryable = sortBy switch
            {
                CategorySortingEnum.NameAsc => queryable.OrderBy(c => c.Name),
                CategorySortingEnum.NameDesc => queryable.OrderByDescending(c => c.Name),
                _ => queryable.OrderBy(c => c.Name)
            };
            return queryable;
        }

        public async Task<bool> IsCategoryIdExist(Guid id)
        {
            return await _categoryRepository.GetTableNoTracking()
                                  .AnyAsync(c => c.Id.Equals(id));
        }
    }
}
