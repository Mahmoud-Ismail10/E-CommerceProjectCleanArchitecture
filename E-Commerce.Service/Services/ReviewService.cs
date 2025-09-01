using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums.Sorting;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class ReviewService : IReviewService
    {
        #region Fields
        private readonly IReviewRepository _reviewRepository;
        #endregion

        #region Constructors
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddReviewAsync(Review review)
        {
            try
            {
                await _reviewRepository.AddAsync(review);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error adding review : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<string> DeleteReviewAsync(Review review)
        {
            try
            {
                await _reviewRepository.DeleteAsync(review);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting review : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<string> EditReviewAsync(Review review)
        {
            try
            {
                await _reviewRepository.UpdateAsync(review);
                return "Success";
            }
            catch (Exception ex)
            {
                Log.Error("Error editing review : {ErrorMessage}", ex.InnerException?.Message ?? ex.Message);
                return "Failed";
            }
        }

        public async Task<Review?> GetReviewByIdsAsync(Guid ProductId, Guid CustomerId)
        {
            return await _reviewRepository.GetTableNoTracking()
                                          .Where(r => r.ProductId == ProductId && r.CustomerId == CustomerId)
                                          .FirstOrDefaultAsync();
        }

        public IQueryable<Review?> FilterReviewPaginatedQueryable(ReviewSortingEnum sortBy, string search, Guid productId)
        {
            var queryable = _reviewRepository.GetTableNoTracking().Where(r => r.ProductId == productId);
            if (!string.IsNullOrWhiteSpace(search))
                queryable = queryable.Where(c => c.Comment!.Contains(search));
            queryable = sortBy switch
            {
                ReviewSortingEnum.CreatedDateAsc => queryable.OrderBy(c => c.CreatedAt),
                ReviewSortingEnum.CreatedDateDesc => queryable.OrderByDescending(c => c.CreatedAt),
                ReviewSortingEnum.RatingAsc => queryable.OrderBy(c => c.Rating),
                ReviewSortingEnum.RatingDesc => queryable.OrderByDescending(c => c.Rating),
                _ => queryable.OrderByDescending(c => c.CreatedAt)
                              .ThenByDescending(c => c.Rating)
            };
            return queryable;
        }
        #endregion
    }
}
