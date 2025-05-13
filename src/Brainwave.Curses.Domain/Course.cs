using Brainwave.Core.DomainObjects;

namespace Brainwave.Curses.Domain
{
    public class Course : Entity, IAggregateRoot
    {
        public Course(string title, string syllabus)
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
        public string Syllabus { get; private set; }

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
            return string.IsNullOrWhiteSpace(Title) == false &&
                   string.IsNullOrWhiteSpace(Syllabus) == false;
        }

    }
}
