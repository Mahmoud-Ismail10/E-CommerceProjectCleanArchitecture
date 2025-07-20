using E_Commerce.Core.Bases;
using MediatR;

namespace E_Commerce.Core.Features.Authorization.Commands.Models
{
    public record DeleteRoleCommand(Guid RoleId) : IRequest<ApiResponse<string>>;
}
