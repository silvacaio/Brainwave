using Brainwave.Core.Data;

namespace Brainwave.ManagementCourses.Domain
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAll();
        Task<Course?> GetById(Guid id);

        void Add(Course Course);
        void Update(Course Course);

        void Add(Lesson lesson);
        void Update(Lesson lesson);
    }
}
