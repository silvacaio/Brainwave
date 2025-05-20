using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementCourses.Domain
{
    public class Lesson : Entity
    {
        public Lesson(Guid CourseId, string title, string content, string material, Guid courseId)
        {
            Title = title;
            Content = content;
            Material = material;
            CourseId = courseId;
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Material { get; private set; }


        public Guid CourseId { get; private set; }

        // EF Rel.
        public Course Course { get; set; }
     
        public override bool IsValid()
        {
            return string.IsNullOrWhiteSpace(Title) == false &&
                   string.IsNullOrWhiteSpace(Content) == false;
        }

        public static class LessonFactory
        {
            public static Lesson New(string title, string content, string material, Guid courseId)
            {
                return new Lesson(Guid.Empty, title, content, material, courseId);
            }
        }

    }
}
