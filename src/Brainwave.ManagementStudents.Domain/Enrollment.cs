using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementStudents.Domain
{
    public class Enrollment : Entity
    {
        public Guid StudentId { get; private set; }
        public Guid CourseId { get; private set; }
        public EnrollmentStatus Status { get; private set; }

        public Enrollment(Guid studentId, Guid courseId, DateTime registrationTime)
        {
            StudentId = studentId;
            CourseId = courseId;
            Status = EnrollmentStatus.PendingPayment;
        }

        public void Activate()
        {
            Status = EnrollmentStatus.Active;
        }

        public static class EnrollmentPendingPayment
        {
            public static Enrollment Create(Guid studentId, Guid courseId)
            {
                var enrollment = new Enrollment(studentId, courseId, DateTime.Now);
                enrollment.Status = EnrollmentStatus.PendingPayment;
                return enrollment;
            }
        }

        public static class EnrollmentActive
        {
            public static Enrollment Create(Guid studentId, Guid courseId)
            {
                var enrollment = new Enrollment(studentId, courseId, DateTime.Now);
                enrollment.Status = EnrollmentStatus.Active;
                return enrollment;
            }
        }

        public static class EnrollmentDone
        {
            public static Enrollment Create(Guid studentId, Guid courseId)
            {
                var enrollment = new Enrollment(studentId, courseId, DateTime.Now);
                enrollment.Status = EnrollmentStatus.Done;
                return enrollment;
            }
        }

        public static class EnrollmentBlocked
        {
            public static Enrollment Create(Guid studentId, Guid courseId)
            {
                var enrollment = new Enrollment(studentId, courseId, DateTime.Now);
                enrollment.Status = EnrollmentStatus.Blocked;
                return enrollment;
            }
        }
    }
}
