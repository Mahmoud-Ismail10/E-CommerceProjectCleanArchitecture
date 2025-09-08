using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Responses;

namespace E_Commerce.Infrastructure.Repositories.Contract
{
    public interface INotificationStore
    {
        Task<string> AddNotification(NotificationResponse notification);
        List<NotificationResponse> GetNotifications(string? receiverId, NotificationReceiverType type);
        Task MarkAllAsRead(string receiverId, NotificationReceiverType type);
        Task MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type);
    }
}
