using E_Commerce.Core.Features.Authentication.Commands.Models;
using E_Commerce.Core.Features.Authentication.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class AuthenticationController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.Authentication.SignIn)]
        public async Task<IActionResult> SignIn([FromForm] SignInCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.Authentication.RefreshToken)]
        public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.Authentication.ValidateToken)]
        public async Task<IActionResult> ValidateToken([FromQuery] AuthorizeUserQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.Authentication.ConfirmEmail)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.Authentication.SendResetPasswordCode)]
        public async Task<IActionResult> SendResetPasswordCode([FromQuery] SendResetPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.Authentication.ConfirmResetPasswordCode)]
        public async Task<IActionResult> ConfirmResetPasswordCode([FromQuery] ConfirmResetPasswordQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPost(Router.Authentication.ResetPassword)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
