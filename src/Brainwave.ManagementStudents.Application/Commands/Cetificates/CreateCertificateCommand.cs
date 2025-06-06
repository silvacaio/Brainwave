using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementStudents.Application.Commands.Cetificates
{
    public class CreateCertificateCommand : Command
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid EnrollmentId { get; set; }

        public CreateCertificateCommand(Guid studentId, Guid courseId, Guid enrollmentId)
        {
            this.StudentId = studentId;
            this.CourseId = courseId;
            this.EnrollmentId = enrollmentId;
        }
        public override bool IsValid()
        {
            ValidationResult = new CreateCertificateCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateCertificateCommandValidation : AbstractValidator<CreateCertificateCommand>
    {
        public CreateCertificateCommandValidation()
        {
            RuleFor(c => c.StudentId)
                .NotEqual(Guid.Empty)
                .WithMessage("The StudentId field cannot be empty.");

            RuleFor(c => c.CourseId)
                .NotEqual(Guid.Empty)
                .WithMessage("The CourseId field cannot be empty.");

            RuleFor(c => c.EnrollmentId)
            .NotEqual(Guid.Empty)
            .WithMessage("The EnrollmentId field cannot be empty.");
        }
    }
}

