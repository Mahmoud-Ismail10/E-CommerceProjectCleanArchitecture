using E_Commerce.Domain.Enums;

namespace E_Commerce.Core.Features.Reviews.Queries.Responses
{
    public record GetReviewPaginatedListResponse
    (string? UserName,
     string? ProductName,
     Rating? Rating,
     string? Comment,
     DateTimeOffset? CreatedAt);
}
