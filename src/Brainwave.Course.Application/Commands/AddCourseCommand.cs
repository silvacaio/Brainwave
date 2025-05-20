using Brainwave.Core.Messages;
using FluentValidation;


namespace Brainwave.Courses.Application.Commands
{
    public class AddCourseCommand : Command
    {
        public AddCourseCommand(string title, string syllabus, Guid userId)
        {
            Title = title;
            Syllabus = syllabus;
            UserId = userId;
        }

        public string Title { get; private set; }
        public string Syllabus { get; private set; }
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

            RuleFor(c => c.Syllabus)
                .NotEmpty()
                .WithMessage("Syllabus is required");

            RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid user");
        }
    }
}
