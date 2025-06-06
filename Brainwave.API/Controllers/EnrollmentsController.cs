using Brainwave.API.Controllers.Base;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Queries;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using Brainwave.ManagementStudents.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Brainwave.API.Controllers
{
    [Route("api/enrollments")]
    public class EnrollmentsController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IStudentQueries _studentQueries;
        private readonly ICourseQueries _courseQueries;

        public EnrollmentsController(INotificationHandler<DomainNotification> notifications,
                                     IStudentQueries studentQueries,
                                     IMediator mediator,
                                     ICourseQueries courseQueries)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _studentQueries = studentQueries;
            _courseQueries = courseQueries;
        }

        [Authorize(Roles = "STUDENT")]
        [HttpPost("{courseId:guid}")]
        public async Task<IActionResult> Add(Guid courseId)
        {
            if (_courseQueries.GetById(courseId) == null)
            {
                NotifyError("Course", "The specified course does not exist.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var command = new AddEnrollmentCommand(UserId, courseId);
            await _mediator.Send(command);

            return CustomResponse();
        }


        [Authorize(Roles = "STUDENT,ADMIN")]
        [HttpPost("{enrollmentId:guid}")]
        public async Task<IActionResult> Get(Guid enrollmentId)
        {
            var enrollment = await _studentQueries.GetEnrollmentById(enrollmentId);
            if (enrollment == null)
            {
                NotifyError("Enrollment", "The specified enrollment does not exist.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(enrollment);
        }
    }

}
