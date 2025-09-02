using E_Commerce.Core.Features.Deliveries.Commands.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize(Roles = "Customer")]
    public class DeliveryController : AppControllerBase
    {
        [HttpPost(Router.DeliveryRouting.SetDeliveryMethod)]
        public async Task<IActionResult> SetDeliveryMethod([FromBody] SetDeliveryMethodCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPut(Router.DeliveryRouting.EditDeliveryMethod)]
        public async Task<IActionResult> EditDeliveryMethod([FromBody] EditDeliveryMethodCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }
    }
}
