using E_Commerce.Core.Features.Products.Commands.Models;
using E_Commerce.Core.Features.Products.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class ProductController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.Paginated)]
        public async Task<IActionResult> GetProductPaginatedList([FromQuery] GetProductPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.GetSingle)]
        public async Task<IActionResult> GetProductById([FromQuery] GetProductByIdQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost(Router.ProductRouting.Create)]
        public async Task<IActionResult> CreateProduct([FromForm] AddProductCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPut(Router.ProductRouting.Edit)]
        public async Task<IActionResult> EditProduct([FromBody] EditProductCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpDelete(Router.ProductRouting.Delete)]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteProductCommand(id)));
        }
    }
}
