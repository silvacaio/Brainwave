using Brainwave.ManagementStudents.Application.Queries.ViewModels;

namespace Brainwave.ManagementStudents.Application.Queries
{
    public interface IStudentQueries
    {
        Task<EnrollmentViewModel?> GetEnrollment(Guid courseId, Guid studentId);
        Task<IEnumerable<EnrollmentViewModel>> GetPendingPaymentEnrollments(Guid studentId);
        Task<CertificateViewModel?> GetCertificate(Guid certificateId, Guid studentId);
        Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsByUserId(Guid userId);
        Task<EnrollmentViewModel> GetEnrollmentById(Guid enrollmentId);
    }
}
