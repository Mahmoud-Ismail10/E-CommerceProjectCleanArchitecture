using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Core.Wrappers
{
    public static class CustomerQueryableExtensions
    {
        public static IQueryable<Customer> ApplyFiltering(
            this IQueryable<Customer> query,
            CustomerSortingEnum? sortBy,
            string? search)
        {
            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.Email.Contains(search));
            }

            // Sort
            query = sortBy switch
            {
                CustomerSortingEnum.FirstNameAsc => query.OrderBy(e => e.FirstName),
                CustomerSortingEnum.FirstNameDesc => query.OrderByDescending(e => e.FirstName),
                CustomerSortingEnum.LastNameAsc => query.OrderBy(e => e.LastName),
                CustomerSortingEnum.LastNameDesc => query.OrderByDescending(e => e.LastName),
                _ => query.OrderBy(e => e.FirstName)
            };
            return query;
        }
    }
}
