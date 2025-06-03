using Brainwave.API.Controllers.Base;
using Brainwave.API.ViewModel;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands;
using Brainwave.ManagementCourses.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Brainwave.API.Controllers
{
    [Route("api/courses")]
    public class CoursesController : MainController
    {
        private readonly IMediator _mediator;
        private readonly ICourseQueries _courseQueries;

        public CoursesController(INotificationHandler<DomainNotification> notifications,
                                 IMediator mediator,
                                 ICourseQueries courseQueries)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _courseQueries = courseQueries;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetAll()
        {
            var courses = await _courseQueries.GetAll();
            return CustomResponse(courses);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CourseViewModel>> GetById(Guid id)
        {
            var course = await _courseQueries.GetById(id);
            return CustomResponse(course);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CourseViewModel course)
        {
            var command = new AddCourseCommand(course.Title, course.SyllabusContent, course.SyllabusDurationInHours, course.SyllabusLanguage, course.Value, UserId);
            await _mediator.Send(command);

            return CustomResponse();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseViewModel course)
        {
            if (id != course.Id)
            {
                NotifyError("Course", "The course ID must match the ID specified in the URL.");
                return CustomResponse();
            }

            var command = new UpdateCourseCommand(course.Id, course.Title, course.SyllabusContent, course.SyllabusDurationInHours, course.SyllabusLanguage, course.Value);
            await _mediator.Send(command);

            return CustomResponse();
        }
      
        [Authorize(Roles = "STUDENT")]
        [HttpPost("{courseId:guid}/make-payment")]
        public async Task<IActionResult> MakePayment(Guid courseId, [FromBody] PaymentViewModel paymentData)
        {
            var command = new MakeCoursePaymentCommand(
                UserId,                
                courseId,
                paymentData.CardHolderName,
                paymentData.CardNumber,
                paymentData.ExpirationDate,
                paymentData.SecurityCode,
                paymentData.Value
            );

            await _mediator.Send(command);
            return CustomResponse();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCourseCommand(id);
            await _mediator.Send(command);

            return CustomResponse();
        }
    }

}
