using E_Commerce.Core.Features.Orders.Commands.Models;
using E_Commerce.Core.Features.Orders.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class OrderController : AppControllerBase
    {
        [Authorize(Roles = "Customer")]
        [HttpGet(Router.OrderRouting.GetMyOrders)]
        public async Task<IActionResult> GetMyOrders([FromQuery] GetMyOrdersQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet(Router.OrderRouting.GetById)]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet(Router.OrderRouting.Paginated)]
        public async Task<IActionResult> GetOrderPaginatedList([FromQuery] GetOrderPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost(Router.OrderRouting.Create)]
        public async Task<IActionResult> CreateOrder()
        {
            var response = await Mediator.Send(new AddOrderCommand());
            return Ok(response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut(Router.OrderRouting.PlaceOrder)]
        public async Task<IActionResult> PlaceOrder([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new PlaceOrderCommand(id));
            return Ok(response);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete(Router.OrderRouting.Delete)]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            var response = await Mediator.Send(new DeleteOrderCommand(id));
            return Ok(response);
        }
    }
}
