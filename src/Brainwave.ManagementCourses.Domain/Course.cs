using Brainwave.Core.DomainObjects;
using Brainwave.ManagementCourses.Domain.ValueObjects;

namespace Brainwave.ManagementCourses.Domain
{
    public class Course : Entity, IAggregateRoot
    {
        public Course(string title, decimal value, Syllabus syllabus)
        {
            Title = title;
            Syllabus = syllabus;
            _lessons = new List<Lesson>();
            Value = value;
            Validate();
        }

        protected Course()
        {
            _lessons = new List<Lesson>();
        }

        protected Course(Guid id, string title, decimal value, Syllabus syllabus) : this(title, value, syllabus)
        {
            Id = id;
        }

        public string Title { get; private set; }
        public decimal Value { get; private set; }

        public Syllabus Syllabus { get; private set; }

        private readonly List<Lesson> _lessons;
        public IReadOnlyCollection<Lesson> Lessons => _lessons;

        public void AddLesson(Lesson item)
        {
            item.AssociateCourse(Id);

            _lessons.Add(item);
        }

        public void Validate()
        {
            Syllabus.Validate();
            Validations.ValidateIfEmpty(Title, "Title is required");
        }


        public static class CourseFactory
        {
            public static Course New(string title, decimal value, Syllabus syllabus)
            {
                return new Course(title, value, syllabus);
            }

            public static Course Update(Guid id, string title, decimal value, Syllabus syllabus)
            {
                return new Course(id, title, value, syllabus);
            }
        }

    }
}
