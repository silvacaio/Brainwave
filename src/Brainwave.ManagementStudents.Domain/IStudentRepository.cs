using Brainwave.Core.Data;

namespace Brainwave.ManagementStudents.Domain
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task Add(Student student);
        Task Update(Student student);

        Task<Student?> GetById(Guid id);

        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentId(Guid id);
        Task Add(Enrollment enrollment);
        Task Update(Enrollment enrollment);
        Task Create(Certificate Certificate);
        Task<Enrollment?> GetEnrollmentByCourseIdAndStudentId(Guid courseId, Guid studentId);
        Task<IEnumerable<Enrollment>> GetPendingPaymentEnrollments(Guid studentId);
        Task<Enrollment?> GetEnrollmentsById(Guid enrollmentId);
        Task<StudentLesson?> GetLessonByStudentIdAndCourseIdAndLessonId(Guid studentId, Guid courseId, Guid lessonId);
        Task Add(StudentLesson newLesson);
        Task<IEnumerable<StudentLesson>> GetStudentLessonsByCourseId(Guid userId, Guid courseId);
    }
}
