using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Reviews.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Reviews.Commands.Handlers
{
    public class ReviewCommandHandler : ApiResponseHandler,
        IRequestHandler<AddReviewCommand, ApiResponse<string>>,
        IRequestHandler<EditReviewCommand, ApiResponse<string>>,
        IRequestHandler<DeleteReviewCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ReviewCommandHandler(
            IReviewService reviewService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var reviewMapper = _mapper.Map<Review>(request);
            var currentCustomerId = _currentUserService.GetUserId();
            reviewMapper.CustomerId = currentCustomerId;
            reviewMapper.CreatedAt = DateTimeOffset.UtcNow.ToLocalTime();
            var result = await _reviewService.AddReviewAsync(reviewMapper);
            if (result == "Success") return Created("");
            else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            var currentCustomerId = _currentUserService.GetUserId();
            var review = await _reviewService.GetReviewByIdsAsync(request.ProductId, currentCustomerId);
            if (review == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.ReviewNotFound]);
            var reviewMapper = _mapper.Map(request, review);
            var result = await _reviewService.EditReviewAsync(reviewMapper);
            if (result == "Success") return Edit("");
            else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var currentCustomerId = _currentUserService.GetUserId();
            var review = await _reviewService.GetReviewByIdsAsync(request.ProductId, currentCustomerId);
            if (review == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.ReviewNotFound]);
            var result = await _reviewService.DeleteReviewAsync(review);
            if (result == "Success") return Deleted<string>();
            else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]);
        }
        #endregion
    }
}
