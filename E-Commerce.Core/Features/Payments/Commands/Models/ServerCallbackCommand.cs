using E_Commerce.Core.Bases;
using MediatR;
using System.Text.Json;

namespace E_Commerce.Core.Features.Payments.Commands.Models
{
    public record ServerCallbackCommand(JsonElement Payload, string Hmac) : IRequest<ApiResponse<string>>;
}
