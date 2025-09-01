using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Reviews.Commands.Models
{
    public record DeleteReviewCommand(Guid ProductId) : IRequest<ApiResponse<string>>;
}
