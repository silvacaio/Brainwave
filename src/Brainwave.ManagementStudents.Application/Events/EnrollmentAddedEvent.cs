using Brainwave.Core.Messages;

namespace Brainwave.ManagementStudents.Application.Events
{
    internal class EnrollmentAddedEvent : Event
    {
        private Guid id;
        private Guid studentId;
        private Guid courseId;

        public EnrollmentAddedEvent(Guid id, Guid studentId, Guid courseId)
        {
            this.id = id;
            this.studentId = studentId;
            this.courseId = courseId;
        }
    }
}