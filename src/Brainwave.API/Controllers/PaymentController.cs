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
    [Route("api/payment")]

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
        [HttpPost("make-payment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentViewModel paymentData)
        {
            var enrollment = await _studentQueries.GetEnrollmentById(paymentData.EnrollmentId);
            if (enrollment == null)
            {
                NotifyError("Enrollment", "The specified enrollment does not exist.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            if (enrollment.StudentId != UserId)
            {
                NotifyError("Enrollment", "You do not have permission to make a payment for this enrollment.");
                return CustomResponse(HttpStatusCode.Forbidden);
            }


            if (enrollment.Status != ManagementStudents.Domain.EnrollmentStatus.PendingPayment)
            {
                NotifyError("Enrollment", "The enrollment is not in a pending payment status.");
                return CustomResponse(HttpStatusCode.BadRequest);
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
