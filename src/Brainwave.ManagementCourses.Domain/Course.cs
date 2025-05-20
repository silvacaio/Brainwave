using Brainwave.Core.DomainObjects;
using Brainwave.ManagementCourses.Domain.ValueObjects;

namespace Brainwave.ManagementCourses.Domain
{
    public class Course : Entity, IAggregateRoot
    {
        public Course(string title, Syllabus syllabus)
        {
            Title = title;
            Syllabus = syllabus;
            _lessons = new List<Lesson>();
        }

        protected Course()
        {
            _lessons = new List<Lesson>();
        }

        public string Title { get; private set; }
        public Syllabus Syllabus { get; private set; }

        private readonly List<Lesson> _lessons;
        public IReadOnlyCollection<Lesson> Lessons => _lessons;

        public void AddLesson(Lesson item)
        {
            if (!item.IsValid()) return;

            item.AssociateCurse(Id);

            _lessons.Add(item);
        }

        public override bool IsValid()
        {
            //TODO: rever esse codigo
            return string.IsNullOrWhiteSpace(Title) == false;
            //   Syllabus.Validate();

        }


        public static class CourseFactory
        {
            public static Course New(string title, Syllabus syllabus)
            {
                return new Course(title, syllabus);
            }
        }

    }
}
