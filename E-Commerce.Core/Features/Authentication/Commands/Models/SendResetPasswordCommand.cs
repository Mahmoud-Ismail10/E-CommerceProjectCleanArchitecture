using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Commands.Models
{
    public record SendResetPasswordCommand(string Email) : IRequest<ApiResponse<string>>;
}
