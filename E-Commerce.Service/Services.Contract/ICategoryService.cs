using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Service.Services.Contract
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetCategoryListAsync();
        IQueryable<Category> FilterCategoryPaginatedQueryable(CategorySortingEnum sortBy, string search);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<string> AddCategoryAsync(Category category);
        Task<string> EditCategoryAsync(Category category);
        Task<string> DeleteCategoryAsync(Category category);
        Task<bool> IsNameExist(string name);
        Task<bool> IsCategoryIdExist(Guid id);
        Task<bool> IsNameExistExcludeSelf(string name, Guid id);
    }
}
