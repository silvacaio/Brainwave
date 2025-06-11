using Brainwave.Core.DomainObjects;
using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Business.ValueObjects;

namespace Brainwave.ManagementPayment.Business
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment(Guid enrollmentId, decimal value)
        {
            EnrollmentId = enrollmentId;
            Value = value;
        }

        public void AddCreditCard(CreditCard creditCard)
        {
            if (creditCard == null || !creditCard.IsValid()) return;
            CreditCard = creditCard;
        }

        public Guid EnrollmentId { get; private set; }
        public decimal Value { get; private set; }

        public CreditCard CreditCard { get; private set; }

        // EF. Rel.

        public PaymentTransaction Transaction { get; private set; }
    }
}
