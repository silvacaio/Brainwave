using Brainwave.Core.Communication.Mediator;
using Brainwave.Core.Messages.IntegrationEvents;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using MediatR;

namespace Brainwave.ManagementStudents.Application.Events
{
    public class EnrollmentEventHandler :
            INotificationHandler<EnrollmentPaidEvent>

    {

        private readonly IMediatorHandler _mediatorHandler;

        public EnrollmentEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(EnrollmentPaidEvent notification, CancellationToken cancellationToken)
        {
            await _mediatorHandler.SendCommand(new EnrollmentPaidCommand(notification.UserId, notification.EnrollmentId, notification.PaymentId, notification.Value));
        }
    }
}
