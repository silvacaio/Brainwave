using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Brainwave.ManagementStudents.Application.Events;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using Brainwave.Core.Messages;
using Brainwave.Core.Communication.Mediator;
using Brainwave.Core.Messages.IntegrationEvents;
using Brainwave.ManagementStudents.Application.Commands.Cetificates;

namespace Brainwave.ManagementStudents.Application.Tests.EventHandlers
{
    public class EnrollmentEventHandlerTests
    {
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly EnrollmentEventHandler _handler;

        public EnrollmentEventHandlerTests()
        {
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _handler = new EnrollmentEventHandler(_mediatorHandlerMock.Object);
        }

        [Fact(DisplayName = "Should handle EnrollmentPaidEvent and send EnrollmentPaidCommand")]
        [Trait("EventHandler", "EnrollmentEventHandler")]
        public async Task Handle_EnrollmentPaidEvent_ShouldSendCommand()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var enrollmentId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            var value = 150;

            var evt = new EnrollmentPaidEvent(userId, enrollmentId, paymentId, value);

            _mediatorHandlerMock
                .Setup(m => m.SendCommand(It.IsAny<EnrollmentPaidCommand>()))
                .ReturnsAsync(true)
                .Verifiable();

            // Act
            await _handler.Handle(evt, CancellationToken.None);

            // Assert
            _mediatorHandlerMock.Verify(m => m.SendCommand(It.Is<EnrollmentPaidCommand>(cmd =>
                cmd.UserId == userId &&
                cmd.EnrollmentId == enrollmentId &&
                cmd.PaymentId == paymentId &&
                cmd.Value == value
            )), Times.Once);
        }

        [Fact(DisplayName = "Should handle EnrollmentFinishedEvent and send CreateCertificateCommand")]
        [Trait("EventHandler", "EnrollmentEventHandler")]
        public async Task Handle_EnrollmentFinishedEvent_ShouldSendCommand()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var enrollmentId = Guid.NewGuid();

            var evt = new EnrollmentFinishedEvent(studentId, courseId, enrollmentId);

            _mediatorHandlerMock
                .Setup(m => m.SendCommand(It.IsAny<CreateCertificateCommand>()))
                .ReturnsAsync(true)
                .Verifiable();

            // Act
            await _handler.Handle(evt, CancellationToken.None);

            // Assert
            _mediatorHandlerMock.Verify(m => m.SendCommand(It.Is<CreateCertificateCommand>(cmd =>
                cmd.StudentId == studentId &&
                cmd.CourseId == courseId &&
                cmd.EnrollmentId == enrollmentId
            )), Times.Once);
        }
    }
}
