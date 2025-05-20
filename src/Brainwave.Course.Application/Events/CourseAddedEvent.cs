using Brainwave.Core.Messages;

namespace Brainwave.Courses.Application.Events
{
    public class CourseAddedEvent : Event
    {
        public CourseAddedEvent(Guid courseId, string title, string syllabus)
        {
            AggregateId = courseId;
            CourseId = courseId;
            Title = title;
            Syllabus = syllabus;
        }

        public Guid CourseId { get; set; }
        public string Title { get; private set; }
        public string Syllabus { get; private set; }
    }
}
