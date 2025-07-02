using Xunit;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Commands.StudentLesson;
using Brainwave.ManagementStudents.Domain;
using Brainwave.Core.Extensions;
using static Brainwave.ManagementStudents.Domain.Enrollment;
using static Brainwave.ManagementStudents.Domain.StudentLesson;

namespace Brainwave.ManagementStudents.Application.Commands.Tests.Commands
{
    public class StudentLessonCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly StudentLessonCommandHandler _handler;
        private readonly Mock<IStudentRepository> _studentRepository;
        private readonly Mock<ICommandValidator> _commandValidator;
        private readonly Mock<IMediator> _mediator;

        public StudentLessonCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _studentRepository = _mocker.GetMock<IStudentRepository>();
            _commandValidator = _mocker.GetMock<ICommandValidator>();
            _mediator = _mocker.GetMock<IMediator>();
            _handler = _mocker.CreateInstance<StudentLessonCommandHandler>();
        }

        [Fact(DisplayName = "Should return false when command is invalid")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            var command = new FinishLessonCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            _commandValidator.Setup(v => v.Validate(command)).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
        }

        [Fact(DisplayName = "Should return false when student is not found")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnFalse_WhenStudentNotFound()
        {
            var command = new FinishLessonCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            _commandValidator.Setup(v => v.Validate(command)).Returns(true);
            _studentRepository.Setup(r => r.GetById(command.StudentId)).ReturnsAsync((Student)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "Student not found."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should return false when student is not enrolled in course")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnFalse_WhenEnrollmentNotFound()
        {
            var studentId = Guid.NewGuid();
            var command = new FinishLessonCommand(studentId, Guid.NewGuid(), Guid.NewGuid());
            var student = Student.StudentFactory.CreateStudent(studentId, "Student");

            _commandValidator.Setup(v => v.Validate(command)).Returns(true);
            _studentRepository.Setup(r => r.GetById(studentId)).ReturnsAsync(student);
            _studentRepository.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(command.CourseId, studentId))
                              .ReturnsAsync((Brainwave.ManagementStudents.Domain.Enrollment)null!);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "Student not enrolled in this course."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should return false when enrollment is already finished")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnFalse_WhenEnrollmentIsDone()
        {
            var enrollment = EnrollmentDone.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishLessonCommand(enrollment.StudentId, enrollment.CourseId, Guid.NewGuid());
            var student = Student.StudentFactory.CreateStudent(enrollment.StudentId, "Student");

            _commandValidator.Setup(v => v.Validate(command)).Returns(true);
            _studentRepository.Setup(r => r.GetById(enrollment.StudentId)).ReturnsAsync(student);
            _studentRepository.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(enrollment.CourseId, enrollment.StudentId))
                              .ReturnsAsync(enrollment);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "This course is already finished."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should return false when enrollment is pending payment")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnFalse_WhenEnrollmentIsPendingPayment()
        {
            var enrollment = EnrollmentPendingPayment.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishLessonCommand(enrollment.StudentId, enrollment.CourseId, Guid.NewGuid());
            var student = Student.StudentFactory.CreateStudent(enrollment.StudentId, "Student");

            _commandValidator.Setup(v => v.Validate(command)).Returns(true);
            _studentRepository.Setup(r => r.GetById(enrollment.StudentId)).ReturnsAsync(student);
            _studentRepository.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(enrollment.CourseId, enrollment.StudentId))
                              .ReturnsAsync(enrollment);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "Student enrollment did not paid."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should return false when lesson already finished")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnFalse_WhenLessonAlreadyExists()
        {
            var enrollment = EnrollmentActive.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishLessonCommand(enrollment.StudentId, enrollment.CourseId, Guid.NewGuid());
            var student = Student.StudentFactory.CreateStudent(enrollment.StudentId, "Student");
            var existingLesson = StudentLessonFactory.Create(enrollment.StudentId, enrollment.CourseId, command.LessonId);

            _commandValidator.Setup(v => v.Validate(command)).Returns(true);
            _studentRepository.Setup(r => r.GetById(enrollment.StudentId)).ReturnsAsync(student);
            _studentRepository.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(enrollment.CourseId, enrollment.StudentId))
                              .ReturnsAsync(enrollment);
            _studentRepository.Setup(r => r.GetLessonByStudentIdAndCourseIdAndLessonId(command.StudentId, command.CourseId, command.LessonId))
                              .ReturnsAsync(existingLesson);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(It.Is<DomainNotification>(n =>
                n.Value == "Student already finish this Lesson."), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should successfully finish lesson")]
        [Trait("StudentLesson", "ManagementStudents - StudentLessonCommandHandler")]
        public async Task FinishLesson_ShouldReturnTrue_WhenValid()
        {
            var enrollment = EnrollmentActive.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishLessonCommand(enrollment.StudentId, enrollment.CourseId, Guid.NewGuid());
            var student = Student.StudentFactory.CreateStudent(enrollment.StudentId, "Student");

            _commandValidator.Setup(v => v.Validate(command)).Returns(true);
            _studentRepository.Setup(r => r.GetById(enrollment.StudentId)).ReturnsAsync(student);
            _studentRepository.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(enrollment.CourseId, enrollment.StudentId))
                              .ReturnsAsync(enrollment);
            _studentRepository.Setup(r => r.GetLessonByStudentIdAndCourseIdAndLessonId(command.StudentId, command.CourseId, command.LessonId))
                              .ReturnsAsync((Domain.StudentLesson)null!);
            _studentRepository.Setup(r => r.Add(It.IsAny<Domain.StudentLesson>())).Returns(Task.CompletedTask);
            _studentRepository.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }
    }
}
