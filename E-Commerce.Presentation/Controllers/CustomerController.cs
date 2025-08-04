using E_Commerce.Core.Features.Customers.Commands.Models;
using E_Commerce.Core.Features.Customers.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    //[Authorize(Roles = "Admin, Employee")]
    public class CustomerController : AppControllerBase
    {
        //[Authorize(Policy = "GetCustomer")]
        [HttpGet(Router.CustomerRouting.Paginated)]
        public async Task<IActionResult> GetCustomerPaginatedList([FromQuery] GetCustomerPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = "GetCustomer")]
        [HttpGet(Router.CustomerRouting.GetById)]
        public async Task<IActionResult> GetCustomerById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetCustomerByIdQuery(id)));
        }

        [Authorize(Policy = "EditCustomer")]
        [HttpPut(Router.CustomerRouting.Edit)]
        public async Task<IActionResult> EditCustomer([FromBody] EditCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = "DeleteCustomer")]
        [HttpDelete(Router.CustomerRouting.Delete)]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteCustomerCommand(id)));
        }
    }
}
