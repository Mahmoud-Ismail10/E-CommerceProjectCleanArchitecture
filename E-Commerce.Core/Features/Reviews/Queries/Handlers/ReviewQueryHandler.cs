using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Reviews.Queries.Models;
using E_Commerce.Core.Features.Reviews.Queries.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace E_Commerce.Core.Features.Reviews.Queries.Handlers
{
    public class ReviewQueryHandler : ApiResponseHandler,
        IRequestHandler<GetReviewPaginatedListQuery, ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ReviewQueryHandler(IReviewService reviewService,
            IProductService productService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _productService = productService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Functions Handle
        public async Task<ApiResponse<PaginatedResult<GetReviewPaginatedListResponse>>> Handle(GetReviewPaginatedListQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product is null) return BadRequest<PaginatedResult<GetReviewPaginatedListResponse>>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            Expression<Func<Review, GetReviewPaginatedListResponse>> expression = c => new GetReviewPaginatedListResponse
            (
                c.Customer!.FirstName + " " + c.Customer.LastName,
                c.Product!.Name,
                c.Rating,
                c.Comment,
                c.CreatedAt
            );

            var filterQuery = _reviewService.FilterReviewPaginatedQueryable(request.SortBy, request.Search!, request.ProductId);
            var paginatedList = await filterQuery.Select(expression!)
                                                 .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return Success(paginatedList);
        }
        #endregion
    }
}
