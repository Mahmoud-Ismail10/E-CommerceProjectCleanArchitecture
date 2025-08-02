using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Products.Queries.Models;
using E_Commerce.Core.Features.Products.Queries.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace E_Commerce.Core.Features.Products.Queries.Handlers
{
    public class ProductQueryHandler : ApiResponseHandler,
        IRequestHandler<GetProductByIdQuery, ApiResponse<GetSingleProductResponse>>,
        IRequestHandler<GetProductPaginatedListQuery, PaginatedResult<GetProductPaginatedListResponse>>
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public ProductQueryHandler(IProductService productService,
            IReviewService reviewService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _reviewService = reviewService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<ApiResponse<GetSingleProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);
            if (product is null) return NotFound<GetSingleProductResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            var productMapper = _mapper.Map<GetSingleProductResponse>(product);

            Expression<Func<Review, ReviewResponse>> expression = review => new ReviewResponse(
                review.CustomerId,
                review.Customer != null ? review.Customer.FirstName + " " + review.Customer.LastName : null,
                review.Rating,
                review.Comment
            );
            var reviewsQueryable = _reviewService.GetReviewsByProductIdQueryable(request.Id);
            var reviewPaginatedList = await reviewsQueryable.Select(expression).ToPaginatedListAsync(request.ReviewPageNumber, request.ReviewPageSize);
            productMapper.Reviews = reviewPaginatedList;

            return Success(productMapper);
        }

        public async Task<PaginatedResult<GetProductPaginatedListResponse>> Handle(GetProductPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, GetProductPaginatedListResponse>> expression = c => new GetProductPaginatedListResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Price = c.Price,
                StockQuantity = c.StockQuantity,
                ImageURL = c.ImageURL,
                CreatedAt = c.CreatedAt,
                CategoryName = c.Category!.Name,
            };

            var filterQuery = _productService.FilterProductPaginatedQueryable(request.SortBy, request.Search);
            var paginatedList = await filterQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
    }
}
