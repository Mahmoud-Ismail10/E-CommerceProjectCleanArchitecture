using E_Commerce.Core.Bases;
using E_Commerce.Domain.Requests;
using MediatR;

namespace E_Commerce.Core.Features.Authorization.Commands.Models
{
    public class UpdateUserRolesCommand : UpdateUserRolesRequest, IRequest<ApiResponse<string>>
    {

    }
}
