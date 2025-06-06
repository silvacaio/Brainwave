using Brainwave.Core.Messages;

namespace Brainwave.ManagementPayment.Application.Commands
{
    public class MakePaymentCommand : Command
    {
        public MakePaymentCommand(Guid userId, Guid EnrollmentId, string cardNumber, string cardHolderName, DateTime expirationDate, string securityCode, decimal value)
        {
            UserId = userId;
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
    }
}
