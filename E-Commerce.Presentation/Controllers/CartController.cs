using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Features.Carts.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class CartController : AppControllerBase
    {
        [HttpGet(Router.CartRouting.GetMyCart)]
        public async Task<IActionResult> GetMyCart()
        {
            return NewResult(await Mediator.Send(new GetMyCartQuery()));
        }

        [HttpGet(Router.CartRouting.GetById)]
        public async Task<IActionResult> GetCartById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCartByIdQuery(id)));
        }

        [HttpPost(Router.CartRouting.AddToCart)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpPut(Router.CartRouting.UpdateItemQuantity)]
        public async Task<IActionResult> UpdateItemQuantity([FromBody] UpdateItemQuantityCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [HttpDelete(Router.CartRouting.RemoveFromCart)]
        public async Task<IActionResult> RemoveFromCart([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new RemoveFromCartCommand(id)));
        }

        [HttpDelete(Router.CartRouting.Delete)]
        public async Task<IActionResult> DeleteCart([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCartCommand(id)));
        }
    }
}
