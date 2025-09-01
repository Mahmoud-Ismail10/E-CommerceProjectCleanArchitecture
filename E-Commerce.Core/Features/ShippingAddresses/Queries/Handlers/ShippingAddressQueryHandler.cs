using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.ShippingAddresses.Queries.Models;
using E_Commerce.Core.Features.ShippingAddresses.Queries.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.ShippingAddresses.Queries.Handlers
{
    public class ShippingAddressQueryHandler : ApiResponseHandler,
        IRequestHandler<GetShippingAddressListQuery, ApiResponse<List<GetShippingAddressListResponse>>>,
        IRequestHandler<GetSingleShippingAddressQuery, ApiResponse<GetSingleShippingAddressResponse>>
    {
        #region Fields
        private readonly IShippingAddressService _shippingAddressService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ShippingAddressQueryHandler(
            IShippingAddressService shippingAddressService,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _shippingAddressService = shippingAddressService;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<List<GetShippingAddressListResponse>>> Handle(GetShippingAddressListQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();
            var shippingAddressList = await _shippingAddressService.GetShippingAddressListByCustomerIdAsync(currentUserId);
            var shippingAddressListMapper = _mapper.Map<List<GetShippingAddressListResponse>>(shippingAddressList);
            return Success(shippingAddressListMapper);
        }

        public async Task<ApiResponse<GetSingleShippingAddressResponse>> Handle(GetSingleShippingAddressQuery request, CancellationToken cancellationToken)
        {
            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.Id);
            if (shippingAddress is null) return NotFound<GetSingleShippingAddressResponse>(_stringLocalizer[SharedResourcesKeys.ShippingAddressDoesNotExist]);
            var shippingAddressMapper = _mapper.Map<GetSingleShippingAddressResponse>(shippingAddress);
            return Success(shippingAddressMapper);
        }
        #endregion
    }
}
