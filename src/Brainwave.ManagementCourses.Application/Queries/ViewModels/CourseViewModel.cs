namespace Brainwave.ManagementCourses.Application.Queries.ViewModels
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SyllabusContent { get; set; }
        public int SyllabusDurationInHours { get; set; }
        public string SyllabusLanguage { get; set; }
        public decimal Value { get; set; }
        public IEnumerable<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();
    }

    public class LessonViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
    }
}
