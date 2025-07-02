using Brainwave.Core.Extensions;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementCourses.Application.Commands.Lesson;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementCourses.Domain.ValueObjects;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class LessonCommandHandlerTests
    {
        private readonly Mock<ICommandValidator> _validatorMock;
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly LessonCommandHandler _handler;

        public LessonCommandHandlerTests()
        {
            _validatorMock = new Mock<ICommandValidator>();
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new LessonCommandHandler(_mediatorMock.Object, _validatorMock.Object, _courseRepositoryMock.Object);
        }

        [Fact(DisplayName = "Should return false when command is invalid")]
        [Trait("Lesson", "ManagementCourses - LessonCommandHandler")]
        public async Task Handle_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new AddLessonCommand("Title", "Content", "Material", Guid.NewGuid());
            _validatorMock.Setup(v => v.Validate(command)).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Should return false when course not found")]
        [Trait("Lesson", "ManagementCourses - LessonCommandHandler")]
        public async Task Handle_ShouldReturnFalse_WhenCourseNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var command = new AddLessonCommand("Title", "Content", "Material", courseId);
            _validatorMock.Setup(v => v.Validate(command)).Returns(true);
            _courseRepositoryMock.Setup(r => r.GetById(courseId, false)).ReturnsAsync((Course)null!);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorMock.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "Course not found."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should return false when lesson with same title already exists")]
        [Trait("Lesson", "ManagementCourses - LessonCommandHandler")]
        public async Task Handle_ShouldReturnFalse_WhenLessonTitleAlreadyExists()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var command = new AddLessonCommand("Duplicate Title", "Content", "Material", courseId);
            var course = Course.CourseFactory.New("Course", 100, new Syllabus("syllabus", 10, "EN"));
            var lesson = Lesson.LessonFactory.New(Guid.NewGuid(), "Title", "Content", null);


            _validatorMock.Setup(v => v.Validate(command)).Returns(true);
            _courseRepositoryMock.Setup(r => r.GetById(courseId, false)).ReturnsAsync(course);
            _courseRepositoryMock.Setup(r => r.GetLessonByCourseIdAndTitle(courseId, command.Title))
                                 .ReturnsAsync(lesson);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mediatorMock.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "A lesson with this title already exists to this course."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should add lesson successfully")]
        [Trait("Lesson", "ManagementCourses - LessonCommandHandler")]
        public async Task Handle_ShouldReturnTrue_WhenCommandIsValid()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var command = new AddLessonCommand("New Lesson", "Content", "Material", courseId);
            var course = Course.CourseFactory.New("Course", 200, new Syllabus("syllabus", 20, "EN"));

            _validatorMock.Setup(v => v.Validate(command)).Returns(true);
            _courseRepositoryMock.Setup(r => r.GetById(courseId, false)).ReturnsAsync(course);
            _courseRepositoryMock.Setup(r => r.GetLessonByCourseIdAndTitle(courseId, command.Title)).ReturnsAsync((Lesson)null!);
            _courseRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _courseRepositoryMock.Verify(r => r.Add(It.IsAny<Lesson>()), Times.Once);
        }
    }
}
