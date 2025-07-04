using Brainwave.Core.DomainObjects;
using Brainwave.ManagementPayment.Application;
using Brainwave.ManagementPayment.Application.ValueObjects;

namespace Brainwave.ManagementPayment.Application
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
            if (creditCard == null) return;
            CreditCard = creditCard;
        }

        public Guid EnrollmentId { get; private set; }
        public decimal Value { get; private set; }

        public CreditCard CreditCard { get; private set; }

        // EF. Rel.

        public PaymentTransaction Transaction { get; private set; }

        //Factory
        public static class PaymentFactory
        {
            public static Payment Create(Guid enrollmentId, decimal value)
            {
                return new Payment(enrollmentId, value);
            }
            public static Payment WithCreditCard(Guid enrollmentId, decimal value, CreditCard creditCard)
            {
                var payment = new Payment(enrollmentId, value);
                payment.AddCreditCard(creditCard);
                return payment;
            }
        }
    }
}
