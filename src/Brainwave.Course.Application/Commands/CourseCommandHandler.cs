using Brainwave.Core.Extensions;
using Brainwave.Core.Messages;
using Brainwave.Courses.Application.Commands;
using Brainwave.Courses.Application.Events;
using Brainwave.Courses.Domain;
using Brainwave.Courses.Domain.ValueObjects;
using MediatR;

namespace Brainwave.Curses.Application.Commands
{
    public class CourseCommandHandler : IRequestHandler<AddCourseCommand, bool>
    {
        private readonly ICommandValidator _commandValidator;
        private readonly ICourseRepository _courseRepository;

        public CourseCommandHandler(ICommandValidator commandValidator, ICourseRepository courseRepository)
        {
            _commandValidator = commandValidator;
            _courseRepository = courseRepository;
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
    }
}
