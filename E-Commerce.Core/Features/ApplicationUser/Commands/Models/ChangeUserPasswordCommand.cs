using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.ApplicationUser.Commands.Models
{
    public record ChangeUserPasswordCommand : IRequest<ApiResponse<string>>
    {
        public Guid Id { get; init; }
        public string CurrentPassword { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmPassword { get; init; }
    }
}
