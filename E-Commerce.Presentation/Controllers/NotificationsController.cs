using E_Commerce.Core.Features.Notifications.Commands.Models;
using E_Commerce.Core.Features.Notifications.Queries.Models;
using E_Commerce.Domain.AppMetaData;
using E_Commerce.Presentation.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    [Authorize]
    public class NotificationsController : AppControllerBase
    {
        [HttpGet(Router.NotificationsRouting.Paginated)]
        public async Task<IActionResult> GetNotificationPaginatedList([FromQuery] GetNotificationPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPut(Router.NotificationsRouting.MarkAsRead)]
        public async Task<IActionResult> EditProduct([FromRoute] string id)
        {
            var response = await Mediator.Send(new EditSingleNotificationToAsReadCommand(id));
            return NewResult(response);
        }
    }
}
