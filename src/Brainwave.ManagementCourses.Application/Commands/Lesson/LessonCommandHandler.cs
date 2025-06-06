using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Events;
using Brainwave.ManagementCourses.Domain;
using MediatR;

namespace Brainwave.ManagementCourses.Application.Commands.Lesson
{
    public class LessonCommandHandler
          : IRequestHandler<AddLessonCommand, bool>
    {

        private readonly ICommandValidator _commandValidator;
        private readonly ICourseRepository _courseRepository;
        private readonly IMediator _mediator;

        public LessonCommandHandler(
            IMediator mediator,
            ICommandValidator commandValidator,
            ICourseRepository courseRepository)
        {
            _commandValidator = commandValidator;
            _courseRepository = courseRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddLessonCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            //validar se o curso existe
            var course = await _courseRepository.GetById(request.CourseId);
            if (course == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Course not found."), cancellationToken);
                return false;
            }

            if (await _courseRepository.GetLessonByCourseIdAndTitle(request.CourseId, request.Title) != null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "A lesson with this title already exists to this course."), cancellationToken);
                return false;
            }

            var lesson = Domain.Lesson.LessonFactory.New(request.CourseId, request.Title, request.Content, request.Material);
            _courseRepository.Add(lesson);

            lesson.AddEvent(new LessonAddedEvent(lesson.Id, lesson.Title, lesson.CourseId));
            return await _courseRepository.UnitOfWork.Commit();
        }
    }
}
