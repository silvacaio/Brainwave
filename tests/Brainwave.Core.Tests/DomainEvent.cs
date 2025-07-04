using System;
using Brainwave.Core.Messages.CommonMessages.DomainEvents;
using Xunit;

namespace Brainwave.Tests.Core.Messages
{
    public class DomainEventTests
    {
        private class TestDomainEvent : DomainEvent
        {
            public TestDomainEvent(Guid aggregateId) : base(aggregateId) { }
        }

        [Fact]
        public void Constructor_ShouldSetAggregateId_AndTimestamp()
        {
            // Arrange
            var aggregateId = Guid.NewGuid();

            // Act
            var domainEvent = new TestDomainEvent(aggregateId);

            // Assert
            Assert.Equal(aggregateId, domainEvent.AggregateId);
            Assert.True((DateTime.Now - domainEvent.Timestamp).TotalSeconds < 1);
        }
    }
}
