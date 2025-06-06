using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Events
{
    public class LessonAddedEvent : Event
    {
        private Guid Id { get; set; }
        private string Title { get; set; }
        private Guid CourseId { get; set; }

        public LessonAddedEvent(Guid id, string title, Guid courseId)
        {
            AggregateId = courseId;
            this.Id = id;
            this.Title = title;
            this.CourseId = courseId;
        }
    }
}
