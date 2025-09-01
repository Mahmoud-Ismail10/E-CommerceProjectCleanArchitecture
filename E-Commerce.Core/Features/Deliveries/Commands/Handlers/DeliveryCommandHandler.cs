using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Deliveries.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace E_Commerce.Core.Features.Deliveries.Commands.Handlers
{
    public class DeliveryCommandHandler : ApiResponseHandler,
        IRequestHandler<SetDeliveryMethodCommand, ApiResponse<string>>,
        IRequestHandler<EditDeliveryMethodCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IOrderService _orderService;
        #endregion

        #region Constructors
        public DeliveryCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
            IOrderService orderService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _orderService = orderService;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(SetDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidOrder]);

            if (order.Delivery == null)
                order.Delivery = new Delivery();

            order.Delivery.DeliveryMethod = request.DeliveryMethod;
            order.Delivery.Status = Status.Draft;

            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            return Success("");
        }

        public async Task<ApiResponse<string>> Handle(EditDeliveryMethodCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.OrderId);
            if (order == null || order.Status != Status.Draft)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidOrder]);

            if (order.Delivery == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            order.Delivery.DeliveryMethod = request.DeliveryMethod;
            order.Delivery.Status = Status.Draft;

            var result = await _orderService.EditOrderAsync(order);
            if (result != "Success")
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            return Success("");
        }
        #endregion
    }
}
