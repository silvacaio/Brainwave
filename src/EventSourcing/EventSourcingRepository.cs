using Brainwave.Core.Data.EventSourcing;
using Brainwave.Core.Messages;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event
        {
            await _eventStoreService.GetConnection().AppendToStreamAsync(
                @event.AggregateId.ToString(),
                ExpectedVersion.Any,
                FormatEvent(@event));
        }

        public async Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId)
        {
            var events = await _eventStoreService.GetConnection()
                .ReadStreamEventsForwardAsync(aggregateId.ToString(), 0, 500, false);

            var eventList = new List<StoredEvent>();

            foreach (var resolvedEvent in events.Events)
            {
                var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var jsonData = JsonConvert.DeserializeObject<BaseEvent>(dataEncoded);

                var storedEvent = new StoredEvent(
                    resolvedEvent.Event.EventId,
                    resolvedEvent.Event.EventType,
                    jsonData.Timestamp,
                    dataEncoded);

                eventList.Add(storedEvent);
            }

            return eventList.OrderBy(e => e.OccurrenceDate);
        }

        private static IEnumerable<EventData> FormatEvent<TEvent>(TEvent @event) where TEvent : Event
        {
            yield return new EventData(
                Guid.NewGuid(),
                @event.MessageType,
                true,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                null);
        }
    }

    internal class BaseEvent
    {
        public DateTime Timestamp { get; set; }
    }
}
