using Brainwave.Core.Messages;

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
    }
}