using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Notifications.Commands.Models
{
    public record EditAllNotificationsToAsReadCommand() : IRequest<ApiResponse<string>>;
}
