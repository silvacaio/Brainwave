using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Events
{
    public class LessonAddedEvent : Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CourseId { get; set; }

        public LessonAddedEvent(Guid id, string title, Guid courseId)
        {
            AggregateId = courseId;
            Id = id;
            Title = title;
            CourseId = courseId;
        }
    }
}
