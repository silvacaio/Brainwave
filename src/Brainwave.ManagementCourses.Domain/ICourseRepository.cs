using Brainwave.Core.Data;

namespace Brainwave.ManagementCourses.Domain
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetAll();
        Task<Course?> GetById(Guid id, bool addLessons = false);

        void Add(Course course);
        void Update(Course course);

        void Add(Lesson lesson);
        void Update(Lesson lesson);
        Task<Course?> GetByTitle(string title);
        void Delete(Course course);
        Task<IEnumerable<Course>> GetCoursesNotIn(Guid[] enrolledCourseIds);
        Task<Lesson?> GetLessonByCourseIdAndTitle(Guid courseId, string title);
        Task<Lesson?> GetLessonByIdAndCourseId(Guid lessonId, Guid courseId);
    }
}
