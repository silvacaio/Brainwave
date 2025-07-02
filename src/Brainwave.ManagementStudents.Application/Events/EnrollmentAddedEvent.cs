using Brainwave.Core.Messages;

namespace Brainwave.ManagementStudents.Application.Events
{
    internal class EnrollmentAddedEvent : Event
    {
        private Guid Id { get; set; }
        private Guid StudentId { get; set; }
        private Guid CourseId { get; set; }

        public EnrollmentAddedEvent(Guid id, Guid studentId, Guid courseId)
        {
            this.Id = id;
            this.StudentId = studentId;
            this.CourseId = courseId;
        }
    }
}