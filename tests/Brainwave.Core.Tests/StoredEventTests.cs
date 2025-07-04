using System;
using Brainwave.Core.Data.EventSourcing;
using Xunit;

namespace Brainwave.Tests.Core.Data
{
    public class StoredEventTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = "UserCreatedEvent";
            var date = DateTime.UtcNow;
            var data = "{ \"name\": \"John\" }";

            // Act
            var storedEvent = new StoredEvent(id, type, date, data);

            // Assert
            Assert.Equal(id, storedEvent.Id);
            Assert.Equal(type, storedEvent.Type);
            Assert.Equal(date, storedEvent.OccurrenceDate);
            Assert.Equal(data, storedEvent.Data);
        }

        [Fact]
        public void OccurrenceDate_ShouldBeMutable()
        {
            // Arrange
            var storedEvent = new StoredEvent(Guid.NewGuid(), "Type", DateTime.UtcNow, "{}");

            var newDate = DateTime.UtcNow.AddDays(1);

            // Act
            storedEvent.OccurrenceDate = newDate;

            // Assert
            Assert.Equal(newDate, storedEvent.OccurrenceDate);
        }
    }
}
