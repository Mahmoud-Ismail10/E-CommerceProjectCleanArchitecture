using E_Commerce.Core.Features.Payments.Commands.Models;
using E_Commerce.Core.Features.Payments.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace E_Commerce.Presentation.Controllers
{
    public class PaymentController : AppControllerBase
    {
        [HttpPost(Router.PaymentRouting.SetPaymentMethod)]
        public async Task<IActionResult> SetPaymentMethod([FromBody] SetPaymentMethodCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPost(Router.PaymentRouting.ServerCallback)]
        public async Task<IActionResult> ServerCallback([FromBody] JsonElement payload)
        {
            var hmac = Request.Query["hmac"].ToString();

            return Ok(await Mediator.Send(new ServerCallbackCommand(payload, hmac)));
        }

        [HttpGet(Router.PaymentRouting.PaymobCallback)]
        public async Task<IActionResult> PaymobCallback(PaymobCallbackQuery query)
        {
            var hmac = Request.Query["hmac"].ToString();

            return Ok(await Mediator.Send(query));
        }
    }
}
