using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class CategoryService : ICategoryService
    {
        #region Fields
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region Contructors
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.AddAsync(category);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error adding category {CategoryName}: {ErrorMessage}", category.Name, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<string> DeleteCategoryAsync(Category category)
        {
            var transaction = await _categoryRepository.BeginTransactionAsync();
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
                Log.Error("Error deleting category {CategoryName}: {ErrorMessage}", category.Name, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<string> EditCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.UpdateAsync(category);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error editing category {CategoryName}: {ErrorMessage}", category.Name, ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetTableNoTracking()
                                              .Where(c => c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            return category;
        }

        public async Task<IReadOnlyList<Category?>> GetCategoryListAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<bool> IsNameExist(string name)
        {
            var category = await _categoryRepository.GetTableNoTracking()
                                              .Where(c => c.Name!.Equals(name))
                                              .FirstOrDefaultAsync();
            if (category != null) return true;
            return false;
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, Guid id)
        {
            var category = await _categoryRepository.GetTableNoTracking()
                                              .Where(c => c.Name!.Equals(name) && !c.Id.Equals(id))
                                              .FirstOrDefaultAsync();
            if (category != null) return true;
            return false;
        }

        public IQueryable<Category?> FilterCategoryPaginatedQueryable(CategorySortingEnum sortBy, string search)
        {
            var queryable = _categoryRepository.GetTableNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
                queryable = queryable.Where(c => c.Name!.Contains(search) || c.Description!.Contains(search));
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
        #endregion
    }
}
