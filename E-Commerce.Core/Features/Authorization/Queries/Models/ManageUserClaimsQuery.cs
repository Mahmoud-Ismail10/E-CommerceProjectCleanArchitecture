using E_Commerce.Core.Bases;
using E_Commerce.Domain.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Authorization.Queries.Models
{
    public record ManageUserClaimsQuery(Guid UserId) : IRequest<ApiResponse<ManageUserClaimsResponse>>;
}
