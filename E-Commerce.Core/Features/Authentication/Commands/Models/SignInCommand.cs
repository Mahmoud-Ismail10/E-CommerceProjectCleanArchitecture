using E_Commerce.Core.Bases;
using E_Commerce.Domain.Helpers;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Commands.Models
{
    public record SignInCommand : IRequest<ApiResponse<JwtAuthResponse>>
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
