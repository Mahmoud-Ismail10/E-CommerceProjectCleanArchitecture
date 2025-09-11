using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Responses;

namespace E_Commerce.Service.Services.Contract
{
    public interface INotificationsService
    {
        Task AddNotificationAsync(NotificationResponse notification);
        IQueryable<NotificationResponse?> GetNotifications(string receiverId, NotificationReceiverType type);
        void MarkAllAsRead(string receiverId, NotificationReceiverType type);
        Task<string> MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type);
    }
}
