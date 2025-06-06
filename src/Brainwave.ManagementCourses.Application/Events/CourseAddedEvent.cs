using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Events
{
    public class CourseAddedEvent : Event
    {
        public CourseAddedEvent(Guid id, string title)
        {
            AggregateId = id;
            Id = id;
            Title = title;
        }

        public Guid Id { get; set; }
        public string Title { get;  set; }
    }
}
