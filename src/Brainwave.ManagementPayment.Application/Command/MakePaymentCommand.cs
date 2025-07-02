using Brainwave.Core.Messages;
using FluentValidation;

namespace Brainwave.ManagementPayment.Application.Commands
{
    public class MakePaymentCommand : Command
    {
        public MakePaymentCommand(Guid userId, Guid enrollmentId, string cardNumber, string cardHolderName, DateTime expirationDate, string securityCode, decimal value)
        {
            UserId = userId;
            EnrollmentId = enrollmentId;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate;
            SecurityCode = securityCode;
            Value = value;
        }

        public Guid UserId { get; set; }
        public Guid EnrollmentId { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Value { get; set; }
        public override bool IsValid()
        {
            ValidationResult = new MakePaymentCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class MakePaymentCommandValidation : AbstractValidator<MakePaymentCommand>
    {
        public MakePaymentCommandValidation()
        {
            RuleFor(c => c.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("UserId field cannot be empty.");

            RuleFor(c => c.EnrollmentId)
               .NotEqual(Guid.Empty)
               .WithMessage("EnrollmentId field cannot be empty.");

            RuleFor(c => c.CardNumber)
                .NotEmpty()
                .WithMessage("CardNumber field cannot be empty.");

            RuleFor(c => c.CardHolderName)
              .NotEmpty()
              .WithMessage("CardHolderName field cannot be empty.");

            RuleFor(c => c.ExpirationDate)
            .GreaterThanOrEqualTo(DateTime.Today.Date)
            .WithMessage("ExpirationDate should be greater or equals than today.");

            RuleFor(c => c.SecurityCode)
             .NotEmpty()
            .WithMessage("SecurityCode field cannot be empty.");

            RuleFor(c => c.Value)
          .GreaterThan(0)
            .WithMessage("Value should be greater than 0.");

        }
    }
}
