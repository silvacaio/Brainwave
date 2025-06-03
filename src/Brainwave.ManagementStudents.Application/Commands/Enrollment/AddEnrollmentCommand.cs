using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementStudents.Application.Commands.Enrollment
{
    public class AddEnrollmentCommand(Guid studentId, Guid courseId) : Command
    {
        public Guid StudentId { get; set; } = studentId;
        public Guid CourseId { get; set; } = courseId;

        public override bool IsValid()
        {
            ValidationResult = new AddEnrollmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddEnrollmentCommandValidation : AbstractValidator<AddEnrollmentCommand>
    {
        public AddEnrollmentCommandValidation()
        {
            RuleFor(c => c.StudentId)
                .NotEqual(Guid.Empty)
                .WithMessage("The StudentId field cannot be empty.");

            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage("The CourseId field cannot be empty.");
        }
    }

}
