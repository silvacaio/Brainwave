using Brainwave.API.Controllers.Base;
using Brainwave.API.ViewModel;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Brainwave.API.Controllers
{
    [Authorize(Roles = "ADMIN,STUDENT")]
    [Route("api/enrollments")]
    public class EnrollmentsController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IStudentQueries _studentQueries;

        public EnrollmentsController(INotificationHandler<DomainNotification> notifications,
                                     IStudentQueries studentQueries,
                                     IMediator mediator)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _studentQueries = studentQueries;
        }

        [HttpGet("pending-payment")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetPendingPaymentEnrollments()
        {
            var enrollments = await _studentQueries.GetPendingPaymentEnrollments(UserId);
            return CustomResponse(enrollments);
        }

        [HttpPost("{courseId:guid}")]
        public async Task<IActionResult> Add(Guid courseId)
        {
            var command = new AddEnrollmentCommand(UserId, courseId);
            await _mediator.Send(command);

            return CustomResponse();
        }
    }

}
