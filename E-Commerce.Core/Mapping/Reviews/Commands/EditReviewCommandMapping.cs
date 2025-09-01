using E_Commerce.Core.Features.Reviews.Commands.Models;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Core.Mapping.Reviews
{
    public partial class ReviewProfile
    {
        public void EditReviewCommandMapping()
        {
            CreateMap<EditReviewCommand, Review>();
        }
    }
}
