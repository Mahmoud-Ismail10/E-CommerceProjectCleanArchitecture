using E_Commerce.Core.Features.ApplicationUser.Commands.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class ApplicationUserController : AppControllerBase
    {
        [HttpPut(Router.UserRouting.ChangePassword)]
        public async Task<IActionResult> ChangePasword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
