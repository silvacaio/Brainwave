using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands;
using Brainwave.ManagementCourses.Application.Events;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementCourses.Domain.ValueObjects;
using MediatR;

namespace Brainwave.ManagementCourses.Application.Commands
{
    public class CourseCommandHandler :
        IRequestHandler<AddCourseCommand, bool>,
       IRequestHandler<AddLessonToCourseCommand, bool>
    {
        private readonly ICommandValidator _commandValidator;
        private readonly ICourseRepository _courseRepository;
        private readonly IMediator _mediator;

        public CourseCommandHandler(ICommandValidator commandValidator, ICourseRepository courseRepository, IMediator mediator)
        {
            _commandValidator = commandValidator;
            _courseRepository = courseRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            //TODO: validar se já existe uma course com o mesmo nome?

            var syllabus = new Syllabus(request.SyllabusContent, request.SyllabusDurationInHours, request.SyllabusLanguage);
            var course = Course.CourseFactory.New(request.Title, syllabus);
            _courseRepository.Add(course);

            course.AddEvent(new CourseAddedEvent(course.Id, course.Title));

            return await _courseRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AddLessonToCourseCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;


            var course = await _courseRepository.GetById(request.CourseId);
            if (course == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Course not found."), cancellationToken);
                return false;
            }


            var lesson = Lesson.LessonFactory.New(request.Title, request.Content, request.Material, request.CourseId);
            _courseRepository.Add(lesson);

            course.AddEvent(new LessonAddedToCourseEvent(course.Id, lesson.Id));

            return await _courseRepository.UnitOfWork.Commit();
        }
    }
}
