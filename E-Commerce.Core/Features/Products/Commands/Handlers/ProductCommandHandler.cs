using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Products.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Products.Commands.Handlers
{
    public class ProductCommandHandler : ApiResponseHandler,
        IRequestHandler<AddProductCommand, ApiResponse<string>>,
        IRequestHandler<EditProductCommand, ApiResponse<string>>,
        IRequestHandler<DeleteProductCommand, ApiResponse<string>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public ProductCommandHandler(IProductService productService, IMapper mapper, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<ApiResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productMapper = _mapper.Map<Product>(request);
            var result = await _productService.AddProductAsync(productMapper);
            if (result == "Success") return Created("");
            else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);
            if (product == null) return NotFound<string>();
            var productMapper = _mapper.Map(request, product);
            var result = await _productService.EditProductAsync(productMapper);
            if (result == "Success") return Edit("");
            else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);
            if (product == null) return NotFound<string>();
            var result = await _productService.DeleteProductAsync(product);
            if (result == "Success") return Deleted<string>();
            else return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]);
        }
    }
}
