using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Queries.Models
{
    public record ConfirmEmailQuery(Guid UserId, string Code) : IRequest<ApiResponse<string>>;
}
