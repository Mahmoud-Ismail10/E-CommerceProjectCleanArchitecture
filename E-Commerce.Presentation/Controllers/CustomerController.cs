using E_Commerce.Core.Features.Customers.Commands.Models;
using E_Commerce.Core.Features.Customers.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class CustomerController : AppControllerBase
    {
        [HttpGet(Router.CustomerRouting.Paginated)]
        public async Task<IActionResult> GetCustomerPaginatedList([FromQuery] GetCustomerPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.CustomerRouting.GetById)]
        public async Task<IActionResult> GetCustomerById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCustomerByIdQuery(id)));
        }

        [HttpPut(Router.CustomerRouting.Edit)]
        public async Task<IActionResult> EditCustomer([FromBody] EditCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.CustomerRouting.Delete)]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCustomerCommand(id)));
        }
    }
}
