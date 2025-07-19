using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Service.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public IQueryable<Review> GetReviewsByProductIdQueryable(Guid productId)
        {
            return _reviewRepository.GetTableNoTracking()
                .Where(r => r.ProductId.Equals(productId))
                .Include(r => r.Customer)
                .AsQueryable();
        }
    }
}
