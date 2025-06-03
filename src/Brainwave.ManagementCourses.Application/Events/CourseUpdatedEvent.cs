using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Events
{
    public class CourseUpdatedEvent : Event
    {
        public CourseUpdatedEvent(Guid courseId, string title)
        {
            AggregateId = courseId;
            Id = courseId;
            Title = title;
        }

        public Guid Id { get; set; }
        public string Title { get; private set; }
    }
}
