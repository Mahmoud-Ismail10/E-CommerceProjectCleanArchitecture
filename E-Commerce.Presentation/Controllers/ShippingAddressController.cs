using E_Commerce.Core.Features.ShippingAddresses.Commands.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class ShippingAddressController : AppControllerBase
    {
        [HttpPost(Router.ShippingAddressRouting.Create)]
        public async Task<IActionResult> AddShippingAddress([FromForm] AddShippingAddressCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
