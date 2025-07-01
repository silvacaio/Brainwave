using Brainwave.Core.DomainObjects;


namespace Brainwave.ManagementStudents.Domain
{
    public class StudentLesson : Entity
    {
        public Guid UserId { get; private set; }
        public Guid LessonId { get; private set; }
        public Guid CourseId { get; private set; }


        public StudentLesson(Guid userId, Guid courseId, Guid lessonId)
        {
            UserId = userId;
            LessonId = lessonId;
            CourseId = courseId;
        }

        public static class StudentLessonFactory
        {
            public static StudentLesson Create(Guid userId, Guid courseId, Guid lessonId)
            {
                return new StudentLesson(userId, courseId, lessonId);
            }
        }
    }
}
