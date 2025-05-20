using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Events
{
    public class LessonAddedToCourseEvent : Event
    {
        public LessonAddedToCourseEvent(Guid courseId, Guid lessonId)
        {
            AggregateId = courseId;
            CourseId = courseId;
            LessonId = lessonId;
        }

        public Guid CourseId { get; set; }
        public Guid LessonId { get; set; }    }
}
