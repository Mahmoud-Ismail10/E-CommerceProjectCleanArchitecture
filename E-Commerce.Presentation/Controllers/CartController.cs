using E_Commerce.Core.Features.Carts.Commands.Models;
using E_Commerce.Core.Features.Carts.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class CartController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.CartRouting.GetMyCart)]
        public async Task<IActionResult> GetMyCart()
        {
            return NewResult(await Mediator.Send(new GetMyCartQuery()));
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet(Router.CartRouting.GetById)]
        public async Task<IActionResult> GetCartById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCartByIdQuery(id)));
        }

        [AllowAnonymous]
        [HttpPost(Router.CartRouting.AddToCart)]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPut(Router.CartRouting.UpdateItemQuantity)]
        public async Task<IActionResult> UpdateItemQuantity([FromBody] UpdateItemQuantityCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

        [AllowAnonymous]
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
