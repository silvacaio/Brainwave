
using Brainwave.ManagementCourses.Application.Queries.ViewModels;

namespace Brainwave.ManagementCourses.Application.Queries
{
    public interface ICourseQueries
    {
        Task<CourseViewModel?> GetById(Guid courseId);
        Task<IEnumerable<CourseViewModel>> GetAll();
        Task<IEnumerable<CourseViewModel>> GetCoursesNotIn(Guid[] enrolledCourseIds);
    }
}
