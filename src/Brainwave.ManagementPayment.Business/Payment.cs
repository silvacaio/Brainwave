using Brainwave.Core.DomainObjects;
using Brainwave.ManagementPayment.Business.ValueObjects;
using System.Transactions;

namespace Brainwave.ManagementPayment.Business
{
    public class Payment : Entity, IAggregateRoot
    {
        public Payment(Guid enrollmentId, decimal value, CreditCard creditCard)
        {
            EnrollmentId = enrollmentId;
            Value = value;
            CreditCard = creditCard;
        }

        public Guid EnrollmentId { get; private set; }
        public decimal Value { get; private set; }

        public CreditCard CreditCard { get; private set; }

        // EF. Rel.

        public Transaction Transaction { get; private set; }
    }
}
