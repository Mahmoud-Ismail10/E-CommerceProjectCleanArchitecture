using E_Commerce.Core.Bases;
using E_Commerce.Domain.Enums;
using MediatR;

namespace E_Commerce.Core.Features.Reviews.Commands.Models
{
    public record AddReviewCommand(Guid ProductId, Rating Rating, string? Comment) : IRequest<ApiResponse<string>>;
}
