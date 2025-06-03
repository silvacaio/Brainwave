using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.Core.Messages;
using Brainwave.ManagementCourses.Application.Commands;
using Brainwave.ManagementCourses.Application.Events;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementCourses.Domain.ValueObjects;
using MediatR;

namespace Brainwave.ManagementCourses.Application.Commands
{
    public class CourseCommandHandler
        : IRequestHandler<AddCourseCommand, bool>,
          IRequestHandler<UpdateCourseCommand, bool>,
          IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly ICommandValidator _commandValidator;
        private readonly ICourseRepository _courseRepository;
        private readonly IMediator _mediator;

        public CourseCommandHandler(
            IMediator mediator,
            ICommandValidator commandValidator,
            ICourseRepository courseRepository)
        {
            _commandValidator = commandValidator;
            _courseRepository = courseRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            if (await _courseRepository.GetByTitle(request.Title) != null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "A course with this title already exists."), cancellationToken);
                return false;
            }

            var syllabus = new Syllabus(request.SyllabusContent, request.SyllabusDurationInHours, request.SyllabusLanguage);
            var course = Course.CourseFactory.New(request.Title, syllabus);
            _courseRepository.Add(course);

            course.AddEvent(new CourseAddedEvent(course.Id, course.Title));

            return await _courseRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;

            var course = await _courseRepository.GetById(request.Id);
            if (course == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Course not found."), cancellationToken);
                return false;
            }

            var syllabus = new Syllabus(request.SyllabusContent, request.SyllabusDurationInHours, request.SyllabusLanguage);
            var updatedCourse = Course.CourseFactory.Update(course.Id, request.Title, syllabus);

            _courseRepository.Update(updatedCourse);
            course.AddEvent(new CourseUpdatedEvent(updatedCourse.Id, updatedCourse.Title));

            return await _courseRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            if (_commandValidator.Validate(request) == false)
                return false;


            var course = await _courseRepository.GetById(request.Id, addLessons: true);
            if (course == null)
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Course not found."), cancellationToken);
                return false;
            }

            if(course.Lessons.Any())
            {
                await _mediator.Publish(new DomainNotification(request.MessageType, "Cannot delete a course that has lessons."), cancellationToken);
                return false;
            }

            _courseRepository.Delete(course);
            course.AddEvent(new CourseDeletedEvent(course.Id, course.Title));

            return await _courseRepository.UnitOfWork.Commit();

        }
    }
}
