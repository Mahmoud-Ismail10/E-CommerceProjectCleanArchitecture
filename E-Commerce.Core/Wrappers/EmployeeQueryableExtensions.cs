using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Core.Wrappers
{
    public static class EmployeeQueryableExtensions
    {
        public static IQueryable<Employee> ApplyFiltering(
            this IQueryable<Employee> query,
            EmployeeSortingEnum? sortBy,
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
                EmployeeSortingEnum.FirstNameAsc => query.OrderBy(e => e.FirstName),
                EmployeeSortingEnum.FirstNameDesc => query.OrderByDescending(e => e.FirstName),
                EmployeeSortingEnum.LastNameAsc => query.OrderBy(e => e.LastName),
                EmployeeSortingEnum.LastNameDesc => query.OrderByDescending(e => e.LastName),
                EmployeeSortingEnum.SalaryAsc => query.OrderBy(e => e.Salary),
                EmployeeSortingEnum.SalaryDesc => query.OrderByDescending(e => e.Salary),
                EmployeeSortingEnum.HireDateAsc => query.OrderBy(e => e.HireDate),
                EmployeeSortingEnum.HireDateDesc => query.OrderByDescending(e => e.HireDate),
                _ => query.OrderBy(e => e.FirstName)
            };

            return query;
        }
    }
}
