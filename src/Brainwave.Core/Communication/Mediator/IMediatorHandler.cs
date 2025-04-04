using Brainwave.Core.Messages.CommonMessages.DomainEvents;
using Brainwave.Core.Messages.CommonMessages.Notifications;
using Brainwave.Core.Messages;

namespace Brainwave.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<bool> SendCommand<T>(T command) where T : Command;
        Task PublishNotification<T>(T notification) where T : DomainNotification;
        Task PublishDomainEvent<T>(T domainEvent) where T : DomainEvent;
    }

}
