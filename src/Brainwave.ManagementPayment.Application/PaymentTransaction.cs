using Brainwave.Core.DomainObjects;
using Brainwave.ManagementPayment.Application;

namespace Brainwave.ManagementPayment.Application
{
    public class PaymentTransaction : Entity
    {
        public PaymentTransaction(Guid enrollmentId, Guid paymentId, decimal value)
        {
            EnrollmentId = enrollmentId;
            PaymentId = paymentId;
            Value = value;
        }

        public Guid EnrollmentId { get; private set; }
        public Guid PaymentId { get; private set; }
        public decimal Value { get; private set; }
        public StatusTransaction StatusTransaction { get; private set; }

        // EF. Rel.
        public Payment Payment { get; set; }

        public static class PaymentTransactionFactory
        {
            public static PaymentTransaction Paid(Guid enrollmentId, Guid paymentId, decimal value)
            {
                return new PaymentTransaction(enrollmentId, paymentId, value)
                {
                    StatusTransaction = StatusTransaction.Paid
                };
            }
            public static PaymentTransaction Refused(Guid enrollmentId, Guid paymentId, decimal value)
            {
                return new PaymentTransaction(enrollmentId, paymentId, value)
                {
                    StatusTransaction = StatusTransaction.Refused
                };
            }
        }
    }
}
