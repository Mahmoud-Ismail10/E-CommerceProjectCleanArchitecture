namespace E_Commerce.Core.Features.Notifications.Queries.Responses
{
    public record GetNotificationPaginatedListResponse
       (string Id,
        string? ReceiverId,
        string? Message,
        DateTimeOffset CreatedAt,
        bool IsRead);
}
