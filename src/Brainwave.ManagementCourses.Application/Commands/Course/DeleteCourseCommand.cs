using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementCourses.Application.Commands.Course
{
    public class DeleteCourseCommand : Command
    {
        public DeleteCourseCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new DeleteCourseCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteCourseCommandValidation : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Course id is required");
        }
    }
}
