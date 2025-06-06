using Brainwave.Core.Messages;

namespace Brainwave.ManagementStudents.Application.Events
{
    public class EnrollmentFinishedEvent : Event
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public Guid EnrollmentId { get; set; }

        public EnrollmentFinishedEvent(Guid studentId, Guid courseId, Guid enrollmentId)
        {
            this.AggregateId = studentId;
            this.StudentId = studentId;
            this.CourseId = courseId;
            this.EnrollmentId = enrollmentId;
        }
    }
}