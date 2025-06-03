using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementCourses.Domain
{
    public class Lesson : Entity
    {
        public Lesson(Guid courseId, string title, string content, string material)
        {
            Title = title;
            Content = content;
            Material = material;
        }

        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Material { get; private set; }


        public Guid CourseId { get; private set; }

        // EF Rel.
        public Course Course { get; set; }

        internal void AssociateCourse(Guid courseId)
        {
            CourseId = courseId;
        }

        public override bool IsValid()
        {
            return string.IsNullOrWhiteSpace(Title) == false &&
                   string.IsNullOrWhiteSpace(Content) == false;
        }
    }
}
