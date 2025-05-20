using Brainwave.Core.Messages;
using FluentValidation;


namespace Brainwave.ManagementCourses.Application.Commands
{
    public class AddCourseCommand : Command
    {
        public AddCourseCommand(string title, string syllabusContent, int syllabusDurationInHours, string syllabusLanguage, Guid userId)
        {
            Title = title;
            SyllabusContent = syllabusContent;
            SyllabusDurationInHours = syllabusDurationInHours;
            SyllabusLanguage = syllabusLanguage;
            UserId = userId;
        }

        public string Title { get; private set; }
        public string SyllabusContent { get; set; }
        public int SyllabusDurationInHours { get; set; }
        public string SyllabusLanguage { get; set; }
        public Guid UserId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddCourseCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddCourseCommandValidation : AbstractValidator<AddCourseCommand>
    {
        public AddCourseCommandValidation()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .WithMessage("Title is required");

            RuleFor(c => c.SyllabusContent)
                .NotEmpty()
                .WithMessage("Syllabus Content is required");

            RuleFor(c => c.SyllabusLanguage)
         .NotEmpty()
         .WithMessage("Syllabus languague is required");

            RuleFor(c => c.SyllabusDurationInHours)
             .GreaterThan(0)
             .WithMessage("Syllabus duration in hours should be greater than 0");

            RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid user");
        }
    }
}
