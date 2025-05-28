using Brainwave.ManagementPayment.Business.ValueObjects;
using System.Transactions;

namespace Brainwave.ManagementPayment.Business
{
    public class Payment
    {
        public Guid CourseId { get; private set; }
        public Guid StudentId { get; private set; }
        public decimal Value { get; private set; }

        public CreditCard CreditCard { get; private set; }

        public Transaction Transaction { get; private set; }
    }
}
