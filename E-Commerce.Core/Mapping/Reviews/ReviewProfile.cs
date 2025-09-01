using AutoMapper;

namespace E_Commerce.Core.Mapping.Reviews
{
    public partial class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            AddReviewCommandMapping();
            EditReviewCommandMapping();
        }
    }
}
