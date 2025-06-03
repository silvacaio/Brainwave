using Brainwave.Core.Messages;

namespace Brainwave.ManagementCourses.Application.Commands
{
    public class MakeCoursePaymentCommand : Command
    {
        public MakeCoursePaymentCommand(Guid studentId, Guid courseId, string cardNumber, string cardHolderName, DateTime expirationDate, string securityCode, decimal value)
        {
            StudentId = studentId;
            CourseId = courseId;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate;
            SecurityCode = securityCode;
            Value = value;
        }

        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        //public Guid EnrollmentId { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Value { get; set; }
    }
}
