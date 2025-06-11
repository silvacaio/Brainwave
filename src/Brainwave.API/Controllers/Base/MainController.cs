using Brainwave.Core.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Security.Claims;

namespace Brainwave.API.Controllers.Base
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notifications;


        protected MainController(
            INotificationHandler<DomainNotification> notifications,
            IMediator mediator)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
        }

        protected Guid UserId => Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        protected bool IsOperationValid()
        {
            return !_notifications.HasNotifications();
        }

        protected ActionResult CustomResponse(object? result = null)
        {
            if (IsOperationValid())
            {
                return Ok(new
                {
                    Success = true,
                    Data = result,
                });
            }

            return BadRequest(new
            {
                Success = false,
                Errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.Publish(new DomainNotification(code, message));
        }

        protected void NotifyError(ModelStateDictionary modelState)
        {
            foreach (var error in modelState.Values.SelectMany(v => v.Errors))
            {
                NotifyError("ModelState", error.ErrorMessage);
            }
        }

        protected void NotifyError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError("Identity", error.Description);
            }
        }
    }

}
