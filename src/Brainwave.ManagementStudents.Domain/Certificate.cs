using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementStudents.Domain
{
    public class Certificate : Entity
    {
        public string StudentName { get; private set; }
        public string CourseName { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid EnrollmentId { get; private set; }

        // EF Relations
        public Student Student { get; set; }
        public Enrollment Enrollment { get; set; }

        public Certificate(string studentName, string courseName, Guid enrollmentId, Guid studentId)
        {
            StudentName = studentName;
            CourseName = courseName;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            Validate();
        }

        private string GetDescription()
        {
            return $"We hereby certify that student {StudentName} successfully completed the course {CourseName} on {CreatedAt:dd/MM/yyyy}.";
        }

        public void Validate()
        {
            if (StudentId == Guid.Empty)
                throw new DomainException("The StudentId field is required.");
            if (EnrollmentId == Guid.Empty)
                throw new DomainException("The EnrollmentId field is required.");
            if (string.IsNullOrWhiteSpace(StudentName))
                throw new DomainException("The Student Name field is required.");
            if (string.IsNullOrWhiteSpace(CourseName))
                throw new DomainException("The Course Name field is required.");
        }
    }
}
