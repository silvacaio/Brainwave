using Brainwave.ManagementStudents.Application.Queries.ViewModels;
using Brainwave.ManagementStudents.Domain;

namespace Brainwave.ManagementStudents.Application.Queries
{
    public interface IStudentQueries
    {
        Task<EnrollmentViewModel?> GetEnrollment(Guid courseId, Guid studentId);
        Task<IEnumerable<EnrollmentViewModel>> GetPendingPaymentEnrollments(Guid studentId);
        Task<CertificateViewModel?> GetCertificate(Guid studentId, Guid enrollmmentId);
        Task<IEnumerable<CertificateViewModel>> GetStudentCertificates(Guid studentId);
        Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsByUserId(Guid userId);
        Task<EnrollmentViewModel> GetEnrollmentById(Guid enrollmentId);
        Task<IEnumerable<StudentLessonViewModel>> GetStudentLessonsByCourseId(Guid userId, Guid id);
    }
}
