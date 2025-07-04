using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementStudents.Application.Commands.Enrollment
{
      public class FinishEnrollmentCommand(Guid courseId, Guid studentId) : Command
    {
        public Guid StudentId { get; set; } = studentId;
        public Guid CourseId { get; set; } = courseId;

        public override bool IsValid()
        {
            ValidationResult = new FinishEnrollmentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinishEnrollmentCommandValidation : AbstractValidator<FinishEnrollmentCommand>
    {
        public FinishEnrollmentCommandValidation()
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
