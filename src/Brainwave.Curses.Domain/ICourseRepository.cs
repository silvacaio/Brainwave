using Brainwave.Core.Data;

namespace Brainwave.Curses.Domain
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAll();
        Task<Course?> GetById(Guid id);

        void Add(Course curse);
        void Update(Course curse);

        void Add(Lesson lesson);
        void Update(Lesson lesson);
    }
}
