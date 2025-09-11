using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Responses;
using E_Commerce.Infrastructure.Repositories.Contract;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Repositories
{
    public class NotificationStore : INotificationStore
    {
        private readonly IDatabase _redisDb;
        private const string NotificationKeyPrefix = "notifications:";

        public NotificationStore(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<string> AddNotification(NotificationResponse notification)
        {
            try
            {
                var key = $"{NotificationKeyPrefix}{notification.ReceiverType}:{notification.ReceiverId}";
                var serialized = JsonSerializer.Serialize(notification);

                // ListLeftPush = Add to start of Redis List
                await _redisDb.ListLeftPushAsync(key, serialized);
                return "Success";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }

        public List<NotificationResponse> GetNotifications(string? receiverId, NotificationReceiverType type)
        {
            var key = $"{NotificationKeyPrefix}{type}:{receiverId}";
            var values = _redisDb.ListRange(key);

            return values
                .Select(v => JsonSerializer.Deserialize<NotificationResponse>(v!))
                .Where(n => n != null)
                .OrderByDescending(n => n!.CreatedAt)
                .ToList()!;
        }

        public async Task MarkAsRead(string notificationId, string receiverId, NotificationReceiverType type)
        {
            var key = $"{NotificationKeyPrefix}{type}:{receiverId}";
            var values = _redisDb.ListRange(key);

            foreach (var v in values)
            {
                var notification = JsonSerializer.Deserialize<NotificationResponse>(v!);
                if (notification != null && notification.Id == notificationId)
                {
                    notification.IsRead = true;

                    // Remove old version & push updated one
                    await _redisDb.ListRemoveAsync(key, v);
                    await _redisDb.ListLeftPushAsync(key, JsonSerializer.Serialize(notification));
                    break;
                }
            }
        }

        public async Task MarkAllAsRead(string receiverId, NotificationReceiverType type)
        {
            var key = $"{NotificationKeyPrefix}{type}:{receiverId}";
            var values = _redisDb.ListRange(key);

            foreach (var v in values)
            {
                var notification = JsonSerializer.Deserialize<NotificationResponse>(v!);
                if (notification != null && !notification.IsRead)
                {
                    notification.IsRead = true;

                    await _redisDb.ListRemoveAsync(key, v);
                    await _redisDb.ListLeftPushAsync(key, JsonSerializer.Serialize(notification));
                }
            }
        }
    }
}
