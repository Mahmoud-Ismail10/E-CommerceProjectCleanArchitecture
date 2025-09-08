using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Notifications.Queries.Models;
using E_Commerce.Core.Features.Notifications.Queries.Responses;
using E_Commerce.Core.Resources;
using E_Commerce.Core.Wrappers;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Helpers;
using E_Commerce.Domain.Responses;
using E_Commerce.Service.Services.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using System.Security.Claims;

namespace E_Commerce.Core.Features.Notifications.Queries.Handlers
{
    public class NotificationsQueryHandler : ApiResponseHandler,
        IRequestHandler<GetNotificationPaginatedListQuery, ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>>
    {
        #region Fields
        private readonly INotificationsService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public NotificationsQueryHandler(
            INotificationsService notificationService,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>> Handle(GetNotificationPaginatedListQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return Unauthorized<PaginatedResult<GetNotificationPaginatedListResponse>>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var userId = user?.FindFirst(nameof(UserClaimModel.Id))?.Value;
            var notifications = role switch
            {
                "Admin" => _notificationService.GetNotifications(userId!, NotificationReceiverType.Admin),
                "Employee" => _notificationService.GetNotifications(userId!, NotificationReceiverType.Employee),
                "Customer" => _notificationService.GetNotifications(userId!, NotificationReceiverType.Customer),
                _ => _notificationService.GetNotifications(userId!, NotificationReceiverType.Unknowen),
            };

            Expression<Func<NotificationResponse, GetNotificationPaginatedListResponse>> expression = c => new GetNotificationPaginatedListResponse
            (
                c.Id,
                c.ReceiverId,
                c.Message,
                c.CreatedAt,
                c.IsRead
            );

            var paginatedList = await notifications.Select(expression!)
                                                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return Success(paginatedList);
        }
        #endregion
    }
}
