using Xunit;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using Brainwave.ManagementStudents.Domain;
using static Brainwave.ManagementStudents.Domain.Enrollment;
using global::Brainwave.Core.Extensions;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class EnrollmentCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly EnrollmentCommandHandler _handler;
        private readonly Mock<IStudentRepository> _studentRepository;
        private readonly Mock<ICommandValidator> _commandValidator;
        private readonly Mock<IMediator> _mediator;

        public EnrollmentCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _studentRepository = _mocker.GetMock<IStudentRepository>();
            _commandValidator = _mocker.GetMock<ICommandValidator>();
            _mediator = _mocker.GetMock<IMediator>();
            _handler = _mocker.CreateInstance<EnrollmentCommandHandler>();
        }

        [Fact(DisplayName = "Should return false when command is invalid")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_AddEnrollment_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            var command = new AddEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());
            _commandValidator.Setup(x => x.Validate(command)).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
        }

        [Fact(DisplayName = "Should add enrollment successfully when valid")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_AddEnrollment_ShouldReturnTrue_WhenValid()
        {
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var command = new AddEnrollmentCommand(studentId, courseId);
            var student = Student.StudentFactory.CreateStudent(studentId, "Student");

            _commandValidator.Setup(x => x.Validate(command)).Returns(true);
            _studentRepository.Setup(x => x.GetById(studentId)).ReturnsAsync(student);
            _studentRepository.Setup(x => x.GetEnrollmentByCourseIdAndStudentId(courseId, studentId)).ReturnsAsync((Enrollment)null!);
            _studentRepository.Setup(x => x.Add(It.IsAny<Enrollment>())).Returns(Task.CompletedTask);
            _studentRepository.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }

        [Fact(DisplayName = "Should return false when enrollment does not exist on payment")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_EnrollmentPaid_ShouldReturnFalse_WhenEnrollmentNotFound()
        {
            var command = new EnrollmentPaidCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100);
            _commandValidator.Setup(x => x.Validate(command)).Returns(true);
            _studentRepository.Setup(x => x.GetEnrollmentsById(command.EnrollmentId)).ReturnsAsync((Enrollment?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(
                It.Is<DomainNotification>(n => n.Value == "Enrollment not found."),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should activate enrollment on valid payment")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_EnrollmentPaid_ShouldReturnTrue_WhenValid()
        {
            var enrollment = EnrollmentPendingPayment.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new EnrollmentPaidCommand(Guid.NewGuid(), enrollment.Id, Guid.NewGuid(), 100);

            _commandValidator.Setup(x => x.Validate(command)).Returns(true);
            _studentRepository.Setup(x => x.GetEnrollmentsById(command.EnrollmentId)).ReturnsAsync(enrollment);
            _studentRepository.Setup(x => x.Update(enrollment)).Returns(Task.CompletedTask);
            _studentRepository.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }

        [Fact(DisplayName = "Should return false when enrollment is not active on finish")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_FinishEnrollment_ShouldReturnFalse_WhenEnrollmentNotActive()
        {
            var enrollment = EnrollmentPendingPayment.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishEnrollmentCommand(enrollment.StudentId, enrollment.CourseId);

            _studentRepository.Setup(x => x.GetEnrollmentByCourseIdAndStudentId(command.CourseId, command.StudentId)).ReturnsAsync(enrollment);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result);
            _mediator.Verify(m => m.Publish(
                It.Is<DomainNotification>(n => n.Value == "Enrollment is not active."),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Should finish enrollment successfully when active")]
        [Trait("Enrollment", " ManagementStudents - EnrollmentCommandHandler")]
        public async Task Handle_FinishEnrollment_ShouldReturnTrue_WhenEnrollmentIsActive()
        {
            var enrollment = EnrollmentActive.Create(Guid.NewGuid(), Guid.NewGuid());
            var command = new FinishEnrollmentCommand(enrollment.StudentId, enrollment.CourseId);

            _studentRepository.Setup(x => x.GetEnrollmentByCourseIdAndStudentId(command.CourseId, command.StudentId)).ReturnsAsync(enrollment);
            _studentRepository.Setup(x => x.Update(enrollment)).Returns(Task.CompletedTask);
            _studentRepository.Setup(x => x.UnitOfWork.Commit()).ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }
    }
}
