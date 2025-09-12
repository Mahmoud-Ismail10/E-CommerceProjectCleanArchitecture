using E_Commerce.Core.Bases;
using E_Commerce.Domain.Helpers;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Commands.Models
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ApiResponse<JwtAuthResponse>>;
}
