using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Notifications.Commands.Models;
using E_Commerce.Core.Resources;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace E_Commerce.Core.Features.Notifications.Commands.Handlers
{
    public class NotificationsCommandHandler : ApiResponseHandler,
        IRequestHandler<EditSingleNotificationToAsReadCommand, ApiResponse<string>>
    {
        #region Fields
        private readonly INotificationsService _notificationsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public NotificationsCommandHandler(
            INotificationsService notificationsService,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _notificationsService = notificationsService;
            _httpContextAccessor = httpContextAccessor;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<string>> Handle(EditSingleNotificationToAsReadCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return Unauthorized<string>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var userId = user?.FindFirst(nameof(UserClaimModel.Id))?.Value;
            var result = role switch
            {
                "Admin" => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Admin),
                "Employee" => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Employee),
                "Customer" => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Customer),
                _ => await _notificationsService.MarkAsRead(request.notificationId, userId!, NotificationReceiverType.Unknowen),
            };

            if (result != "Success") return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToMarkNotifyAsRead]);
            return Success("");
        }
        #endregion
    }
}
