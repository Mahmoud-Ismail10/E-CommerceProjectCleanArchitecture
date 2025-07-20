using E_Commerce.Core.Features.Authorization.Commands.Models;
using E_Commerce.Core.Features.Authorization.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : AppControllerBase
    {
        [HttpGet(Router.Authorization.GetRoleById)]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetRoleByIdQuery(id)));
        }

        [HttpGet(Router.Authorization.GetAllRoles)]
        public async Task<IActionResult> GetRoleList()
        {
            var response = await Mediator.Send(new GetRoleListQuery());
            return Ok(response);
        }

        [HttpPost(Router.Authorization.CreateRole)]
        public async Task<IActionResult> CreateRole([FromForm] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.Authorization.EditRole)]
        public async Task<IActionResult> EditRole([FromForm] EditRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.Authorization.DeleteRole)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteRoleCommand(id)));
        }
    }
}
