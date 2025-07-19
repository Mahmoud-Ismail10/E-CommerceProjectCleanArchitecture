using E_Commerce.Core.Features.Categories.Commands.Models;
using E_Commerce.Core.Features.Categories.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class CategoryController : AppControllerBase
    {
        [HttpGet(Router.CategoryRouting.GetAll)]
        public async Task<IActionResult> GetCategoryList()
        {
            var response = await Mediator.Send(new GetCategoryListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.Paginated)]
        public async Task<IActionResult> GetCategoryPaginatedList([FromQuery] GetCategoryPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.CategoryRouting.GetById)]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
        }

        [HttpPost(Router.CategoryRouting.Create)]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.CategoryRouting.Edit)]
        public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.CategoryRouting.Delete)]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCategoryCommand(id)));
        }
    }
}
