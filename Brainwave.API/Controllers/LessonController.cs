using Brainwave.API.Controllers.Base;
using Brainwave.API.ViewModel;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands.Lesson;
using Brainwave.ManagementCourses.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Brainwave.API.Controllers
{
    public class LessonController : MainController
    {
        private readonly IMediator _mediator;
        private readonly ICourseQueries _courseQueries;
        public LessonController(INotificationHandler<DomainNotification> notifications,
            IMediator mediator,
            ICourseQueries courseQueries)
            : base(notifications, mediator)
        {
            _mediator = mediator;
            _courseQueries = courseQueries; 
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] LessonViewModel lesson)
        {
            var course = await _courseQueries.GetById(lesson.CourseId);
            if (course == null)
            {
                NotifyError("Course", "The specified course does not exist.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var command = new AddLessonCommand(lesson.Title, lesson.Content, lesson.Material, lesson.CourseId);
            await _mediator.Send(command);

            return CustomResponse();
        }
    }
}
