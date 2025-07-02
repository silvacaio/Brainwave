using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands.Course;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementCourses.Domain.ValueObjects;
using MediatR;
using Moq;
using Moq.AutoMock;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class CourseCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly CourseCommandHandler _handler;

        public CourseCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CourseCommandHandler>();
        }

        [Fact(DisplayName = "Should not add course if command is invalid")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task AddCourse_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            var command = new AddCourseCommand("", "", 0, "", 0, Guid.NewGuid());
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
        }

        [Fact(DisplayName = "Should not add course if title already exists")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task AddCourse_ShouldReturnFalse_WhenTitleAlreadyExists()
        {
            var course = Domain.Course.CourseFactory.New("Old", 100, new Syllabus("Old", 5, "EN"));

            var command = new AddCourseCommand("Course X", "Content", 10, "EN", 100, Guid.NewGuid());
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetByTitle(command.Title)).ReturnsAsync(course);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.Is<DomainNotification>(n => n.Value.Contains("already exists")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should add course successfully")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task AddCourse_ShouldReturnTrue_WhenValid()
        {
            var command = new AddCourseCommand("Course X", "Content", 10, "EN", 100, Guid.NewGuid());
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetByTitle(command.Title)).ReturnsAsync((Course)null);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }

        [Fact(DisplayName = "Should not update course if not found")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task UpdateCourse_ShouldReturnFalse_WhenCourseNotFound()
        {
            var command = new UpdateCourseCommand(Guid.NewGuid(), "Title", "Content", 10, "EN", 100);
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetById(command.Id, false)).ReturnsAsync((Course)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.Is<DomainNotification>(n => n.Value == "Course not found."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should update course successfully")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task UpdateCourse_ShouldReturnTrue_WhenValid()
        {
            var course = Domain.Course.CourseFactory.New("Old", 100, new Syllabus("Old", 5, "EN"));
            var command = new UpdateCourseCommand(course.Id, "New", "Content", 10, "EN", 200);

            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetById(course.Id, false)).ReturnsAsync(course);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }

        [Fact(DisplayName = "Should not delete course if not found")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task DeleteCourse_ShouldReturnFalse_WhenCourseNotFound()
        {
            var command = new DeleteCourseCommand(Guid.NewGuid());
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetById(command.Id, true)).ReturnsAsync((Domain.Course)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.Is<DomainNotification>(n => n.Value == "Course not found."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should not delete course if it has lessons")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task DeleteCourse_ShouldReturnFalse_WhenCourseHasLessons()
        {
            var course = Domain.Course.CourseFactory.New("Title", 100, new Syllabus("Content", 5, "EN"));
            var lesson = Lesson.LessonFactory.New(Guid.NewGuid(), "Title", "Content", null);
            course.AddLesson(lesson);

            var command = new DeleteCourseCommand(course.Id);
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetById(command.Id, true)).ReturnsAsync(course);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.Is<DomainNotification>(n => n.Value == "Cannot delete a course that has lessons."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should delete course successfully when no lessons")]
        [Trait("Course", "ManagementCourses - CourseCommandHandler")]
        public async Task DeleteCourse_ShouldReturnTrue_WhenNoLessons()
        {
            var course = Domain.Course.CourseFactory.New("Title", 100, new Syllabus("Content", 5, "EN"));
            var command = new DeleteCourseCommand(course.Id);

            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.GetById(command.Id, true)).ReturnsAsync(course);
            _mocker.GetMock<ICourseRepository>().Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }
    }
}
