using Brainwave.Core.Data.EventSourcing;
using Brainwave.Core.Messages.CommonMessages.DomainEvents;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.Core.Messages;
using MediatR;

namespace Brainwave.Core.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator,
                               IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            await _mediator.Publish(@event);
            await _eventSourcingRepository.SaveEvent(@event);
        }

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification);
        }

        public async Task PublishDomainEvent<T>(T domainEvent) where T : DomainEvent
        {
            await _mediator.Publish(domainEvent);
        }
    }

}
