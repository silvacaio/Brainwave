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
            var enrollment = await _studentRepository.GetEnrollmentByCourseIdAndStudentId(courseId, studentId);
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

        public static EnrollmentViewModel CreateEnrollmentViewModel(Enrollment enrollment)
        {
            return new EnrollmentViewModel
            {
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId,
                Status = enrollment.Status,
                CreatedAt = enrollment.CreatedAt,
            };
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetEnrollmentsByUserId(Guid userId)
        {
            var enrollments = await _studentRepository.GetEnrollmentsByStudentId(userId);

            if (enrollments == null)
                return Enumerable.Empty<EnrollmentViewModel>();

            return enrollments.Select(CreateEnrollmentViewModel).ToList();
        }

        public async Task<EnrollmentViewModel> GetEnrollmentById(Guid enrollmentId)
        {
            var enrollment = await _studentRepository.GetEnrollmentsById(enrollmentId);
            if (enrollment == null)
                return null;

            return CreateEnrollmentViewModel(enrollment);
        }

        public async Task<IEnumerable<StudentLessonViewModel>> GetStudentLessonsByCourseId(Guid userId, Guid courseId)
        {
            var studentLessons = await _studentRepository.GetStudentLessonsByCourseId(userId, courseId);
            if (studentLessons == null)
                return Enumerable.Empty<StudentLessonViewModel>();

            return studentLessons.Select(CreateEnrollmentViewModel).ToList();
        }

        public static StudentLessonViewModel CreateEnrollmentViewModel(StudentLesson studentLesson)
        {
            return new StudentLessonViewModel
            {
                StudentId = studentLesson.UserId,
                CourseId = studentLesson.CourseId,
                LessonId = studentLesson.LessonId
            };
        }
    }
}

