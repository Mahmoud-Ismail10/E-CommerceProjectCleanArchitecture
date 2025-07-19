using E_Commerce.Domain.Entities;

namespace E_Commerce.Service.Services.Contract
{
    public interface IReviewService
    {
        IQueryable<Review> GetReviewsByProductIdQueryable(Guid productId);
    }
}
