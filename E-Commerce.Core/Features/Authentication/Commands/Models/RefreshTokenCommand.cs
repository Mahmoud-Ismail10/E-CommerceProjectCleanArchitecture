using E_Commerce.Core.Bases;
using E_Commerce.Domain.Helpers;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Commands.Models
{
    public record RefreshTokenCommand : IRequest<ApiResponse<JwtAuthResponse>>
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
