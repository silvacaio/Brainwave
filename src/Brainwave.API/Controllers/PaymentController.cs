using Brainwave.API.Controllers.Base;
using Brainwave.API.ViewModel;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands;
using Brainwave.ManagementCourses.Application.Queries;
using Brainwave.ManagementPayment.Application.Commands;
using Brainwave.ManagementStudents.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Brainwave.API.Controllers
{
    public class PaymentController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IStudentQueries _studentQueries;

        public PaymentController(INotificationHandler<DomainNotification> notifications, IMediator mediator, IStudentQueries studentQueries)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _studentQueries = studentQueries;
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet("pending-payment")]
        public async Task<ActionResult<IEnumerable<EnrollmentViewModel>>> GetPendingPaymentEnrollments()
        {
            var enrollments = await _studentQueries.GetPendingPaymentEnrollments(UserId);
            return CustomResponse(enrollments);
        }

        [Authorize(Roles = "STUDENT")]
        [HttpPost("{courseId:guid}/make-payment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentViewModel paymentData)
        {
            if (_studentQueries.GetEnrollmentById(paymentData.EnrollmentId) == null)
            {
                NotifyError("Enrollment", "The specified enrollment does not exist.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var command = new MakePaymentCommand(
                UserId,
                paymentData.EnrollmentId,
                paymentData.CardHolderName,
                paymentData.CardNumber,
                paymentData.ExpirationDate,
                paymentData.SecurityCode,
                paymentData.Value
            );

            await _mediator.Send(command);
            return CustomResponse();
        }
    }
}
