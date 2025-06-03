using Brainwave.Core.Data;

namespace Brainwave.ManagementCourses.Domain
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAll();
        Task<Course?> GetById(Guid id, bool addLessons = false);

        void Add(Course curse);
        void Update(Course curse);

        void Add(Lesson lesson);
        void Update(Lesson lesson);
        Task<Course?> GetByTitle(string title);
        void Delete(Course course);
    }
}
