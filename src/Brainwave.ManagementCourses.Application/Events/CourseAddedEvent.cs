using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Events
{
    public class CourseAddedEvent : Event
    {
        public CourseAddedEvent(Guid courseId, string title)
        {
            AggregateId = courseId;
            CourseId = courseId;
            Title = title;
        }

        public Guid CourseId { get; set; }
        public string Title { get; private set; }
    }
}
