using Brainwave.ManagementCourses.Application.Queries.ViewModels;
using Brainwave.ManagementCourses.Domain;

namespace Brainwave.ManagementCourses.Application.Queries
{
    public class CourseQueries : ICourseQueries
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

            return courses.Select(CreateCourseViewModel).ToList();

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

        public async Task<IEnumerable<CourseViewModel>> GetCoursesNotIn(Guid[] enrolledCourseIds)
        {
            var courses = await _courseRepository.GetCoursesNotIn(enrolledCourseIds);
            if (courses == null || !courses.Any())
                return Enumerable.Empty<CourseViewModel>();

            return courses.Select(CreateCourseViewModel).ToList();

        }

        public async Task<LessonViewModel> GetLessonByCourseIdAndLessonId(Guid courseId, Guid lessonId)
        {
            var lesson = await _courseRepository.GetLessonByIdAndCourseId(lessonId, courseId);
            if (lesson == null)
                return null;

            return CreateLessonViewModel(lesson);
        }

        private LessonViewModel CreateLessonViewModel(Lesson lesson)
        {
            return new LessonViewModel
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Content = lesson.Content,
                Material = lesson.Material
            };
        }
    }
}
