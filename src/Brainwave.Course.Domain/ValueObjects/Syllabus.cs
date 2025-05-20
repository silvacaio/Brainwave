using Brainwave.Core.DomainObjects;

namespace Brainwave.Courses.Domain.ValueObjects
{
    public class Syllabus
    {
        public Syllabus(string content, int durationInHours, string language)
        {
            //TODO: dever esse trecho

            //Validations.ValidateIfEmpty(content, "Content is required");
            //Validations.ValidateIfLessThan(durationInHours, 1, "Duration in hours should be greater than 0"); ;
            //Validations.ValidateIfEmpty(language, "Language is required");

            Content = content;
            DurationInHours = durationInHours;
            Language = language;

            Validate();
        }

        public string Content { get; set; }
        public int DurationInHours { get; set; }
        public string Language { get; set; }

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
