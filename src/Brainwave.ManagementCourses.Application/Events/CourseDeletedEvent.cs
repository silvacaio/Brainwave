using Brainwave.Core.Messages;
using Brainwave.ManagementCourses.Domain;

namespace Brainwave.ManagementCourses.Application.Events
{

    public class CourseDeletedEvent : Event
    {
        public CourseDeletedEvent(Guid id, string title)
        {
            AggregateId = id;
            Id = id;
            Title = title;
        }

        public Guid Id { get; set; }
        public string Title { get; private set; }
    }
}
