using Brainwave.Core.Data;
using Brainwave.ManagementStudents.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.ManagementStudents.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentsContext _context;
        protected readonly DbSet<Student> _studentDbSet;
        protected readonly DbSet<Enrollment> _enrollmentDbSet;
        protected readonly DbSet<StudentLesson> _studentLessonDbSet;
        protected readonly DbSet<Certificate> _certificationDbSet;


        public StudentRepository(StudentsContext context)
        {
            _context = context;
            _studentDbSet = _context.Set<Student>();
            _enrollmentDbSet = _context.Set<Enrollment>();
            _studentLessonDbSet = _context.Set<StudentLesson>();
            _certificationDbSet = _context.Set<Certificate>();
            _studentDbSet.AsNoTracking(); 
            _enrollmentDbSet.AsNoTracking();
            _studentLessonDbSet.AsNoTracking();
            _certificationDbSet.AsNoTracking();
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task Add(Student student)
        {
            await _studentDbSet.AddAsync(student);
        }

        public async Task Update(Student student)
        {
            _studentDbSet.Update(student);
        }

        public async Task<Student?> GetById(Guid id)
        {
            return await _studentDbSet.FindAsync(id);
        }

        public async Task Add(Enrollment enrollment)
        {
            await _enrollmentDbSet.AddAsync(enrollment);
        }

        public async Task Update(Enrollment enrollment)
        {
            _enrollmentDbSet.Update(enrollment);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Create(Certificate Certificate)
        {
            await _context.Certificates.AddAsync(Certificate);
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentId(Guid studentId)
        {
            return await _enrollmentDbSet.Where(e => e.StudentId == studentId).ToListAsync();
        }

        public async Task<Enrollment?> GetEnrollmentByCourseIdAndStudentId(Guid courseId, Guid studentId)
        {
            return await _enrollmentDbSet.FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);
        }

        public async Task<IEnumerable<Enrollment>> GetPendingPaymentEnrollments(Guid studentId)
        {
            return await _enrollmentDbSet.Where(e => e.StudentId == studentId && e.Status == EnrollmentStatus.PendingPayment).ToListAsync();
        }

        public async Task<Enrollment?> GetEnrollmentsById(Guid enrollmentId)
        {
            return await _enrollmentDbSet.FindAsync(enrollmentId);
        }

        public async Task<StudentLesson?> GetLessonByStudentIdAndCourseIdAndLessonId(Guid studentId, Guid courseId, Guid lessonId)
        {
            return await _studentLessonDbSet.FirstOrDefaultAsync(s => s.UserId == studentId && s.CourseId == courseId && s.LessonId == lessonId);
        }

        public async Task Add(StudentLesson newLesson)
        {
            await _studentLessonDbSet.AddAsync(newLesson);
        }

        public async Task<IEnumerable<StudentLesson>> GetStudentLessonsByCourseId(Guid userId, Guid courseId)
        {
            return await _studentLessonDbSet.Where(s => s.UserId == userId && s.CourseId == courseId).ToListAsync();
        }

        public async Task<IEnumerable<Certificate>> GetStudentCertificates(Guid studentId)
        {
            return await _certificationDbSet.Where(s => s.StudentId == studentId).ToListAsync();
        }

        public async Task<Certificate?> GetCertificate(Guid studentId, Guid enrollmentId)
        {
            return await _certificationDbSet.FirstOrDefaultAsync(s => s.StudentId == studentId && s.EnrollmentId == enrollmentId);
        }
    }
}
