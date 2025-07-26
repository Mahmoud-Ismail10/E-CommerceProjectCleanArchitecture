using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Queries.Models
{
    public record ConfirmResetPasswordQuery(string Code, string Email) : IRequest<ApiResponse<string>>;
}
