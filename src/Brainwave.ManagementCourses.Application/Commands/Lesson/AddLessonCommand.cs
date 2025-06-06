using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementCourses.Application.Commands.Lesson
{
    public class AddLessonCommand : Command
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CourseId { get; set; }
        public string? Material { get; set; }

        public AddLessonCommand(string title, string content,
                                string material, Guid courseId)
        {
            AggregateId = courseId;
            Title = title;
            Content = content;
            CourseId = courseId;
            Material = material;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddLessonCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddLessonCommandValidation : AbstractValidator<AddLessonCommand>
    {
        public AddLessonCommandValidation()
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .WithMessage("The Title field is required.");
            RuleFor(c => c.Content)
                .NotEmpty()
                .WithMessage("The Content field is required.");

            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage("The CourseId field is required.");
        }
    }

}
