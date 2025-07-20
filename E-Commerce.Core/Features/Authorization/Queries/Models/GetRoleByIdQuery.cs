using E_Commerce.Core.Bases;
using E_Commerce.Core.Features.Authorization.Queries.Responses;
using MediatR;

namespace E_Commerce.Core.Features.Authorization.Queries.Models
{
    public record GetRoleByIdQuery(Guid Id) : IRequest<ApiResponse<GetSingleRoleResponse>>;
}
