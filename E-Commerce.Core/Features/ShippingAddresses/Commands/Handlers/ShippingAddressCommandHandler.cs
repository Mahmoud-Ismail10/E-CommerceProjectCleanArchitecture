using AutoMapper;
using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.ShippingAddresses.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.AuthService.Services.Contract;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.ShippingAddresses.Commands.Handlers
{
    public class ShippingAddressCommandHandler : ApiResponseHandler,
        IRequestHandler<AddShippingAddressCommand, ApiResponse<string>>,
        IRequestHandler<SetShippingAddressCommand, ApiResponse<string>>,
        IRequestHandler<EditShippingAddressCommand, ApiResponse<string>>,
        IRequestHandler<DeleteShippingAddressCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IShippingAddressService _shippingAddressService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public ShippingAddressCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IShippingAddressService shippingAddressService,
            IOrderService orderService,
            IMapper mapper,
            ICurrentUserService currentUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _shippingAddressService = shippingAddressService;
            _orderService = orderService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();
            var shippingAddressMapper = _mapper.Map<ShippingAddress>(request);
            shippingAddressMapper.CustomerId = currentUserId;
            var result = await _shippingAddressService.AddShippingAddressAsync(shippingAddressMapper);
            if (result == "Success") return Created("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.CreateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(SetShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidOrder]);

            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.ShippingAddressId);
            if (shippingAddress == null)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.ShippingAddressDoesNotExist]);

            var deliveryOffset = DeliveryTimeCalculator.Calculate(shippingAddress.City, order.Delivery!.DeliveryMethod);
            var deliveryCost = DeliveryCostCalculator.Calculate(shippingAddress.City, order.Delivery.DeliveryMethod);

            order.ShippingAddressId = request.ShippingAddressId;
            order.Delivery.Description = $"Delivery for order #{order.Id} to {shippingAddress.State}, {shippingAddress.City}, {shippingAddress.Street}";
            order.Delivery.DeliveryTime = DateTime.UtcNow.Add(deliveryOffset);
            order.Delivery.Cost = deliveryCost;

            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            return Success("");
        }

        public async Task<ApiResponse<string>> Handle(EditShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.Id);
            if (shippingAddress == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.ShippingAddressDoesNotExist]);
            var shippingAddressMapper = _mapper.Map<ShippingAddress>(request);
            shippingAddressMapper.CustomerId = shippingAddress.CustomerId;
            var result = await _shippingAddressService.EditShippingAddressAsync(shippingAddressMapper);
            if (result == "Success") return Edit("");
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
        }

        public async Task<ApiResponse<string>> Handle(DeleteShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var shippingAddress = await _shippingAddressService.GetShippingAddressByIdAsync(request.Id);
            if (shippingAddress == null) return NotFound<string>(_stringLocalizer[SharedResourcesKeys.ShippingAddressDoesNotExist]);
            var result = await _shippingAddressService.DeleteShippingAddressAsync(shippingAddress);
            if (result == "Success") return Deleted<string>();
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.DeleteFailed]);
        }
        #endregion
    }
}
