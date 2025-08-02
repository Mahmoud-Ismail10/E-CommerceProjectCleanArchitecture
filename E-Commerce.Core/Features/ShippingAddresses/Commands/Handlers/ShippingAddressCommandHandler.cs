using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.ShippingAddresses.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.ShippingAddresses.Commands.Handlers
{
    public class ShippingAddressCommandHandler : ApiResponseHandler,
        IRequestHandler<AddShippingAddressCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IShippingAddressService _shippingAddressService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public ShippingAddressCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IShippingAddressService shippingAddressService,
            IMapper mapper,
            ICurrentUserService currentUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _shippingAddressService = shippingAddressService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();
            request.CustomerId = currentUserId;
            var shippingAddressMapper = _mapper.Map<ShippingAddress>(request);
            var result = await _shippingAddressService.AddShippingAddressAsync(shippingAddressMapper);
            if (result == "Success") return Created("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
        }
        #endregion
    }
}
