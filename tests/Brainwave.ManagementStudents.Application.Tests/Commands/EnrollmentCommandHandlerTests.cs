using Xunit;
using Moq.AutoMock;
using System;
using System.Threading;
using System.Threading.Tasks;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using Brainwave.ManagementStudents.Domain;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using MediatR;
using static Brainwave.ManagementStudents.Domain.Enrollment;
using Moq;
using Brainwave.Core.Extensions;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class EnrollmentCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly EnrollmentCommandHandler _handler;

        public EnrollmentCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<EnrollmentCommandHandler>();
        }

        [Fact(DisplayName = "Should fail when command is invalid")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_AddEnrollment_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new AddEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Should add enrollment successfully when valid")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_AddEnrollment_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var command = new AddEnrollmentCommand(studentId, courseId);
            var student = Student.StudentFactory.CreateStudent(studentId, "Student");

            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.GetById(studentId)).ReturnsAsync(student);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.GetEnrollmentByCourseIdAndStudentId(courseId, studentId)).ReturnsAsync((Enrollment)null!);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.Add(It.IsAny<Enrollment>())).Returns(Task.CompletedTask);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should fail if enrollment not found on payment")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_EnrollmentPaid_ShouldReturnFalse_WhenEnrollmentNotFound()
        {
            // Arrange
            var command = new EnrollmentPaidCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100);
            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.GetEnrollmentsById(command.EnrollmentId)).ReturnsAsync((Enrollment?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(
                It.Is<DomainNotification>(n => n.Value == "Enrollment not found."),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should mark enrollment as paid")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_EnrollmentPaid_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var enrollment = EnrollmentPendingPayment.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new EnrollmentPaidCommand(Guid.NewGuid(), enrollment.Id, Guid.NewGuid(), 100);

            _mocker.GetMock<ICommandValidator>().Setup(x => x.Validate(command)).Returns(true);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.GetEnrollmentsById(command.EnrollmentId)).ReturnsAsync(enrollment);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.Update(enrollment)).Returns(Task.CompletedTask);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should not finish inactive enrollment")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_FinishEnrollment_ShouldReturnFalse_WhenEnrollmentIsNotActive()
        {
            // Arrange
            var enrollment = EnrollmentPendingPayment.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishEnrollmentCommand(enrollment.StudentId, enrollment.CourseId);

            _mocker.GetMock<IStudentRepository>()
                .Setup(x => x.GetEnrollmentByCourseIdAndStudentId(command.CourseId, command.StudentId))
                .ReturnsAsync(enrollment);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(
                It.Is<DomainNotification>(n => n.Value == "Enrollment is not active."),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should finish active enrollment successfully")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_FinishEnrollment_ShouldReturnTrue_WhenValid()
        {
            // Arrange
            var enrollment = EnrollmentActive.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishEnrollmentCommand(enrollment.StudentId, enrollment.CourseId);

            _mocker.GetMock<IStudentRepository>()
                .Setup(x => x.GetEnrollmentByCourseIdAndStudentId(command.CourseId, command.StudentId))
                .ReturnsAsync(enrollment);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.Update(enrollment)).Returns(Task.CompletedTask);
            _mocker.GetMock<IStudentRepository>().Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}

