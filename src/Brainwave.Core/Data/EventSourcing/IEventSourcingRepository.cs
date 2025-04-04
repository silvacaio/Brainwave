using Brainwave.Core.Messages;

namespace Brainwave.Core.Data.EventSourcing
{
    public interface IEventSourcingRepository
    {
        Task SaveEvent<TEvent>(TEvent evento) where TEvent : Event;
        Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId);
    }
}
