using Brainwave.Core.DomainObjects;
using Brainwave.ManagementPayment.Business;

namespace Brainwave.ManagementPayment.Application
{
    public class PaymentTransaction : Entity
    {
        public Guid EnrollmentId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Value { get; set; }
        public StatusTransaction StatusTransaction { get; set; }

        // EF. Rel.
        public Payment Payment { get; set; }
    }
}
