using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace E_Commerce.Core.Wrappers
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
            where T : class
        {
            if (source == null)
            {
                throw new Exception("Empty");
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            if (source is IQueryable<T> queryable && queryable.Provider is IAsyncQueryProvider)
            {
                // EF Core (Async)
                int count = await queryable.AsNoTracking().CountAsync();
                if (count == 0)
                    return PaginatedResult<T>.Success(new List<T>(), count, pageNumber, pageSize);

                var items = await queryable
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
            }
            else
            {
                // Redis (Sync LINQ)
                int count = source.Count();
                if (count == 0)
                    return PaginatedResult<T>.Success(new List<T>(), count, pageNumber, pageSize);

                var items = source
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
            }
        }
    }
}
