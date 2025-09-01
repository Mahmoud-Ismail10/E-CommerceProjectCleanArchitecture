using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;

namespace E_Commerce.Service.Services.Contract
{
    public interface IReviewService
    {
        Task<string> AddReviewAsync(Review review);
        Task<string> DeleteReviewAsync(Review review);
        Task<string> EditReviewAsync(Review review);
        IQueryable<Review?> FilterReviewPaginatedQueryable(ReviewSortingEnum sortBy, string search, Guid productId);
        Task<Review?> GetReviewByIdsAsync(Guid ProductId, Guid CustomerId);
    }
}
