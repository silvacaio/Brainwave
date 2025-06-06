namespace Brainwave.Core.Messages.IntegrationEvents
{
    public class EnrollmentPaidEvent : Event
    {
        public EnrollmentPaidEvent(Guid userId, Guid enrollmentId, Guid paymentId, decimal value)
        {
            AggregateId = userId;
            UserId = userId;
            EnrollmentId = enrollmentId;
            PaymentId = paymentId;
            Value = value;
        }

        public Guid UserId { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Value { get; set; }

    }
}
