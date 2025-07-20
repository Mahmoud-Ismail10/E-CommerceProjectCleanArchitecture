using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authorization.Commands.Models
{
    public record EditRoleCommand(Guid RoleId, string RoleName) : IRequest<ApiResponse<string>>;
}
