namespace Brainwave.API.ViewModel
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SyllabusContent { get; set; }
        public int SyllabusDurationInHours { get; set; }
        public string SyllabusLanguage { get; set; }
        public decimal Value { get; set; }
    }
}
