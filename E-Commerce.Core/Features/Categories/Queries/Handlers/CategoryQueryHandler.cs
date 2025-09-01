using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Categories.Queries.Models;
using E_Commerce.Core.Features.Categories.Queries.Response;
using E_Commerce.Core.Resources;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace E_Commerce.Core.Features.Categories.Queries.Handlers
{
    public class CategoryQueryHandler : ApiResponseHandler
        , IRequestHandler<GetCategoryListQuery, ApiResponse<List<GetCategoryListResponse>>>
        , IRequestHandler<GetCategoryByIdQuery, ApiResponse<GetSingleCategoryResponse>>
        , IRequestHandler<GetCategoryPaginatedListQuery, PaginatedResult<GetCategoryPaginatedListResponse>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CategoryQueryHandler(ICategoryService categoryService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<List<GetCategoryListResponse>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categoryList = await _categoryService.GetCategoryListAsync();
            var categoryListMapper = _mapper.Map<List<GetCategoryListResponse>>(categoryList);
            return Success(categoryListMapper);
        }

        public async Task<ApiResponse<GetSingleCategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category is null) return NotFound<GetSingleCategoryResponse>(_stringLocalizer[SharedResourcesKeys.CategoryNotFound]);
            var categoryMapper = _mapper.Map<GetSingleCategoryResponse>(category);
            return Success(categoryMapper);
        }

        public async Task<PaginatedResult<GetCategoryPaginatedListResponse>> Handle(GetCategoryPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Category, GetCategoryPaginatedListResponse>> expression = c => new GetCategoryPaginatedListResponse(c.Id, c.Name!, c.Description);
            var filterQuery = _categoryService.FilterCategoryPaginatedQueryable(request.SortBy, request.Search!);
            var paginatedList = await filterQuery.Select(expression!).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }
        #endregion
    }
}
