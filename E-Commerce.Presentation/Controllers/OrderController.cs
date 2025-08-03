using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Core.Features.Orders.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class OrderController : AppControllerBase
    {
        [HttpGet(Router.OrderRouting.GetById)]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id));
            return NewResult(response);
        }

        [HttpGet(Router.OrderRouting.Paginated)]
        public async Task<IActionResult> GetOrderPaginatedList([FromQuery] GetOrderPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost(Router.OrderRouting.Create)]
        public async Task<IActionResult> CreateOrder([FromBody] AddOrderCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
