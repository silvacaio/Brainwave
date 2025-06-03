using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementCourses.Application.Commands.Course
{
    public class UpdateCourseCommand : Command
    {
        public UpdateCourseCommand(Guid id, string title, string syllabusContent, int syllabusDurationInHours, string syllabusLanguage, decimal value)
        {
            Id = id;
            Title = title;
            SyllabusContent = syllabusContent;
            SyllabusDurationInHours = syllabusDurationInHours;
            SyllabusLanguage = syllabusLanguage;
            Value = value;
        }

        public Guid Id { get;  set; }
        public string Title { get;  set; }
        public string SyllabusContent { get; set; }
        public int SyllabusDurationInHours { get; set; }
        public string SyllabusLanguage { get; set; }
        public decimal Value { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCourseCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateCourseCommandValidation : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidation()
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

            RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid course id");

            RuleFor(c => c.Value)
           .GreaterThan(0)
            .WithMessage("Value should be greater than 0");
        }
    }
}
