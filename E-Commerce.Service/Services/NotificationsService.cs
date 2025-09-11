using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Responses;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;
using Serilog;

namespace E_Commerce.Service.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationStore _notificationStore;
        private readonly INotificationSender _notificationSender;

        public NotificationsService(
            INotificationStore notificationStore,
            INotificationSender notificationSender)
        {
            _notificationStore = notificationStore;
            _notificationSender = notificationSender;
        }

        public async Task AddNotificationAsync(NotificationResponse notification)
        {
            var result = await _notificationStore.AddNotification(notification);
            if (result == "Success")
                await _notificationSender.SendToUserAsync(notification.ReceiverId!, notification.Message!);
            else
                Log.Warning("Failed to add notification for ReceiverId: {ReceiverId}", notification.ReceiverId);
        }

        public IQueryable<NotificationResponse?> GetNotifications(string receiverId, NotificationReceiverType type)
        {
            return _notificationStore.GetNotifications(receiverId, type).AsQueryable();
        }

        public async Task<string> MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type)
        {
            try
            {
                await _notificationStore.MarkAsRead(notificationId, receiverId, type);
                return "Success";
            }
            catch (Exception)
            {
                Log.Warning("Failed to mark notification as read for ReceiverId: {ReceiverId}", receiverId);
                return "Failed";
            }
        }

        public void MarkAllAsRead(string receiverId, NotificationReceiverType type)
        {
            _notificationStore.MarkAllAsRead(receiverId, type);
        }
    }
}
