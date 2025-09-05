using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Responses;

namespace E_Commerce.Infrastructure.Repositories.Contract
{
    public interface INotificationStore
    {
        void AddNotification(NotificationResponse notification);
        List<NotificationResponse> GetNotifications(string? receiverId, NotificationReceiverType type);
        void MarkAllAsRead(string receiverId, NotificationReceiverType type);
        void MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type);
    }
}
