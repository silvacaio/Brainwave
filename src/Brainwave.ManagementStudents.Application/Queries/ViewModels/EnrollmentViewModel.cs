using Brainwave.ManagementStudents.Domain;

namespace Brainwave.ManagementStudents.Application.Queries.ViewModels
{
    public class EnrollmentViewModel
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CertificateViewModel
    {
        public byte[] File { get; set; }
    }
}
