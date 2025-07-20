using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Categories.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Categories.Commands.Handlers
{
    public class CategoryCommandHandler : ApiResponseHandler
       , IRequestHandler<AddCategoryCommand, ApiResponse<string>>
       , IRequestHandler<EditCategoryCommand, ApiResponse<string>>
       , IRequestHandler<DeleteCategoryCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CategoryCommandHandler(ICategoryService categoryService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryMapper = _mapper.Map<Category>(request);
            var result = await _categoryService.AddCategoryAsync(categoryMapper);
            /// check for existing categories before Fluant Validation
            //if (result == "Exist") return UnprocessableEntity<string>("Category already exists");
            if (result == "Success") return Created("");
            else return BadRequest<string>();
        }

        public async Task<ApiResponse<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category == null) return NotFound<string>();
            var categoryMapper = _mapper.Map(request, category);
            var result = await _categoryService.EditCategoryAsync(categoryMapper);
            if (result == "Success") return Edit("");
            else return BadRequest<string>();
        }

        public async Task<ApiResponse<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(request.Id);
            if (category == null) return NotFound<string>();
            var result = await _categoryService.DeleteCategoryAsync(category);
            if (result == "Success") return Deleted<string>();
            else return BadRequest<string>();
        }
        #endregion
    }
}
