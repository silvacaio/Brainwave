using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementStudents.Domain
{
    public class Certificate : Entity
    {
        public string StudentName { get; private set; }
        public string CourseName { get; private set; }
        public DateTime CompletionDate { get; private set; }
        public byte[] File { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid EnrollmentId { get; private set; }

        // EF Relations
        public Student Student { get; set; }
        public Enrollment Enrollment { get; set; }

        public Certificate(string studentName, string courseName, Guid enrollmentId, Guid studentId, DateTime completionDate)
        {
            StudentName = studentName;
            CourseName = courseName;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            CompletionDate = completionDate;
            Validate();
        }

        private string GetDescription()
        {
            return $"We hereby certify that student {StudentName} successfully completed the course {CourseName} on {CompletionDate:dd/MM/yyyy}.";
        }

        public void AddFile(byte[] file)
        {
            //TODO: é realmente obrigatorio
            if (file == null || file.Length == 0)
                throw new DomainException("Invalid certificate file.");

            File = file;
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
