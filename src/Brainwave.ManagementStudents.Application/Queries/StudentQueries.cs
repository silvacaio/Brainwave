using Brainwave.ManagementStudents.Application.Queries.ViewModels;
using Brainwave.ManagementStudents.Domain;

namespace Brainwave.ManagementStudents.Application.Queries
{
    public class StudentQueries : IStudentQueries
    {
        private readonly IStudentRepository _studentRepository;

        public StudentQueries(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Task<CertificateViewModel?> GetCertificate(Guid certificateId, Guid studentId)
        {
            throw new NotImplementedException();
        }

        public async Task<EnrollmentViewModel?> GetEnrollment(Guid courseId, Guid studentId)
        {
            var enrollment = await _studentRepository.GetEnrollmentByStudentIdAndCourseId(courseId, studentId);
            if (enrollment == null)
                return null;

            return CreateEnrollmentViewModel(enrollment);
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetPendingPaymentEnrollments(Guid studentId)
        {
            var enrollments = await _studentRepository.GetPendingPaymentEnrollments(studentId);
            if (enrollments == null)
                return Enumerable.Empty<EnrollmentViewModel>();

            return enrollments.Select(CreateEnrollmentViewModel).ToList();
        }

        private static EnrollmentViewModel CreateEnrollmentViewModel(Enrollment enrollment)
        {
            return new EnrollmentViewModel
            {
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId,
                Status = enrollment.Status,
                CreatedAt = enrollment.CreatedAt,
            };
        }
    }
}
