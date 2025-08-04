using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Features.Carts.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class CartController : AppControllerBase
    {
        [HttpGet(Router.CartRouting.GetById)]
        public async Task<IActionResult> GetCartById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCartByIdQuery(id)));
        }

        [HttpPost(Router.CartRouting.Edit)]
        public async Task<IActionResult> UpdateCart([FromBody] EditCartCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.CartRouting.Delete)]
        public async Task<IActionResult> DeleteCart([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCartCommand(id)));
        }
    }
}
