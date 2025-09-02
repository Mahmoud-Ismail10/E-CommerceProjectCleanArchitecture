using E_Commerce.Core.Features.Employees.Commands.Models;
using E_Commerce.Core.Features.Employees.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeeController : AppControllerBase
    {
        [Authorize(Roles = "Admin,Employee", Policy = "GetEmployee")]
        [HttpGet(Router.EmployeeRouting.GetById)]
        public async Task<IActionResult> GetEmployeeById([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new GetEmployeeByIdQuery(id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.EmployeeRouting.Paginated)]
        public async Task<IActionResult> GetEmployeePaginatedList([FromQuery] GetEmployeePaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.EmployeeRouting.Create)]
        public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin, Employee", Policy = "EditEmployee")]
        [HttpPut(Router.EmployeeRouting.Edit)]
        public async Task<IActionResult> EditEmployee([FromBody] EditEmployeeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.EmployeeRouting.Delete)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            return NewResult(await Mediator.Send(new DeleteEmployeeCommand(id)));
        }
    }
}
