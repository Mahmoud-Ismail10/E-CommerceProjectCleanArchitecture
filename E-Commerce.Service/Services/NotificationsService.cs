using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Responses;
using E_Commerce.Infrastructure.Repositories.Contract;
using E_Commerce.Service.Services.Contract;

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
            _notificationStore.AddNotification(notification);

            await _notificationSender.SendToUserAsync(notification.ReceiverId!, notification.Message!);
        }

        public List<NotificationResponse> GetNotifications(string receiverId, NotificationReceiverType type)
        {
            return _notificationStore.GetNotifications(receiverId, type);
        }

        public void MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type)
        {
            _notificationStore.MarkAsRead(notificationId, receiverId, type);
        }

        public void MarkAllAsRead(string receiverId, NotificationReceiverType type)
        {
            _notificationStore.MarkAllAsRead(receiverId, type);
        }
    }
}
