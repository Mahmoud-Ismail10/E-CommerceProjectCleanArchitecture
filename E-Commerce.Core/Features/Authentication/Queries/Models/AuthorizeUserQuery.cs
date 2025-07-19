using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authentication.Queries.Models
{
    public record AuthorizeUserQuery : IRequest<ApiResponse<string>>
    {
        public string AccessToken { get; init; }
    }
}
