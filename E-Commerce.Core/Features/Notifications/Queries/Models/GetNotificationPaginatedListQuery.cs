using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Notifications.Queries.Responses;
using E_Commerce.Core.Wrappers;
using MediatR;

namespace E_Commerce.Core.Features.Notifications.Queries.Models
{
    public record GetNotificationPaginatedListQuery(int PageNumber, int PageSize) :
        IRequest<ApiResponse<PaginatedResult<GetNotificationPaginatedListResponse>>>;
}
