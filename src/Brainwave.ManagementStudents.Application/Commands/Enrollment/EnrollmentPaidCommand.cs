using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementStudents.Application.Commands.Enrollment
{
    public class EnrollmentPaidCommand : Command
    {
        public Guid UserId { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Value { get; set; }

        public EnrollmentPaidCommand(Guid userId, Guid enrollmentId, Guid paymentId, decimal value)
        {
            AggregateId = userId;
            UserId = userId;
            EnrollmentId = enrollmentId;
            PaymentId = paymentId;
            Value = value;
        }

        public override bool IsValid()
        {
            ValidationResult = new EnrollmentPaidCommanddValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class EnrollmentPaidCommanddValidation : AbstractValidator<EnrollmentPaidCommand>
    {
        public EnrollmentPaidCommanddValidation()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("The UserId field cannot be empty.");

            RuleFor(c => c.EnrollmentId)
                .NotEqual(Guid.Empty)
                .WithMessage("The EnrollmentId field cannot be empty.");

            RuleFor(c => c.PaymentId)
                .NotEqual(Guid.Empty)
                .WithMessage("The PaymentId field cannot be empty.");
        }
    }
}