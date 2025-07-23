using E_Commerce.Core.Features.Authorization.Commands.Models;
using E_Commerce.Core.Features.Authorization.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [SwaggerOperation(Summary = " ادارة صلاحيات المستخدمين", OperationId = "ManageUserRoles")]
        [HttpGet(Router.Authorization.ManageUserRoles)]
        public async Task<IActionResult> ManageUserRoles([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ManageUserRolesQuery(id)));
        }

        [SwaggerOperation(Summary = " تعديل صلاحيات المستخدمين", OperationId = "UpdateUserRoles")]
        [HttpPut(Router.Authorization.UpdateUserRoles)]
        public async Task<IActionResult> UpdateUserRoles([FromBody] UpdateUserRolesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [SwaggerOperation(Summary = " ادارة صلاحيات الاستخدام للمستخدمين", OperationId = "ManageUserClaims")]
        [HttpGet(Router.Authorization.ManageUserClaims)]
        public async Task<IActionResult> ManageUserClaims([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new ManageUserClaimsQuery(id)));
        }

        [SwaggerOperation(Summary = " تعديل صلاحيات الاستخدام للمستخدمين", OperationId = "UpdateUserClaims")]
        [HttpPut(Router.Authorization.UpdateUserClaims)]
        public async Task<IActionResult> UpdateUserClaims([FromBody] UpdateUserClaimsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
