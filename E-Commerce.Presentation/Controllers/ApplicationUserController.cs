using E_Commerce.Core.Features.ApplicationUser.Commands.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class ApplicationUserController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.UserRouting.Register)]
        public async Task<IActionResult> Register([FromBody] AddCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.UserRouting.ChangePassword)]
        public async Task<IActionResult> ChangePasword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
