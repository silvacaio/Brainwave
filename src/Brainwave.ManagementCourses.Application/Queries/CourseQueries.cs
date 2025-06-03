using Brainwave.ManagementCourses.Application.Queries.ViewModels;
using Brainwave.ManagementCourses.Domain;

namespace Brainwave.ManagementCourses.Application.Queries
{
    public class CourseQueries
    {
        private readonly ICourseRepository _courseRepository;

        public CourseQueries(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseViewModel?> GetById(Guid courseId)
        {
            var course = await _courseRepository.GetById(courseId);

            if (course == null)
                return null;

            return CreateCourseViewModel(course);
        }

        public async Task<IEnumerable<CourseViewModel>> GetAll()
        {
            var courses = await _courseRepository.GetAll();

            return courses.Select(course => CreateCourseViewModel(course)).ToList();

        }

        public static CourseViewModel CreateCourseViewModel(Course course)
        {
            return new CourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                SyllabusContent = course.Syllabus.Content,
                SyllabusLanguage = course.Syllabus.Language,
                SyllabusDurationInHours = course.Syllabus.DurationInHours,
                Value = course.Value,
                Lessons = course.Lessons.Select(lesson => new LessonViewModel
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    Content = lesson.Content,
                    Material = lesson.Material
                }).ToList()
            };
        }

    }
}
