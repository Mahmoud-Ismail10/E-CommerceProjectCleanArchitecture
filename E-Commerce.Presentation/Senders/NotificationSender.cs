using E_Commerce.Presentation.Hubs;
using E_Commerce.Service.Services.Contract;
using Microsoft.AspNetCore.SignalR;

namespace E_Commerce.Presentation.Senders
{
    public class NotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NotificationSender(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendToUserAsync(string userId, string notification)
        {
            return _hubContext.Clients.User(userId)
                .SendAsync("ReceiveNotification", notification);
        }
    }
}
