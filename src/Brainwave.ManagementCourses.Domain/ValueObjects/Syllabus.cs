using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementCourses.Domain.ValueObjects
{
    public class Syllabus 
    {
        public Syllabus(string content, int durationInHours, string language)
        {
            Content = content;
            DurationInHours = durationInHours;
            Language = language;

            Validate();
        }

        public string Content { get; private set; }
        public int DurationInHours { get; private set; }
        public string Language { get; private set; }

        public override string ToString()
        {
            return $"Content: {Content}. Durantion in hours: {DurationInHours}. Language: {Language}";
        }

        internal void Validate()
        {
            Validations.ValidateIfEmpty(Content, "Content is required");
            Validations.ValidateIfLessThan(DurationInHours, 1, "Duration in hours should be greater than 0"); ;
            Validations.ValidateIfEmpty(Language, "Language is required");
        }
    }
}
