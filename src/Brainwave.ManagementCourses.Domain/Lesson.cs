using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementCourses.Domain
{
    public class Lesson : Entity
    {
        public Lesson(Guid courseId, string title, string content, string? material)
        {
            CourseId = courseId;
            Title = title;
            Content = content;
            Material = material;

            Validate();
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public string? Material { get; private set; }

        public Guid CourseId { get; private set; }

        // EF Rel.
        public Course Course { get; set; }

        public void AssociateCourse(Guid courseId)
        {
            CourseId = courseId;
        }

        private void Validate()
        {
            Validations.ValidateIfEmpty(Title, "Title is required");
            Validations.ValidateIfEmpty(Content, "Content is required");
        }  

        public static class LessonFactory
        {
            public static Lesson New(Guid courseId, string title, string content, string? material)
            {
                return new Lesson(courseId, title, content, material);
            }
        }
    }
}
