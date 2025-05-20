using Brainwave.Core.Data;
using Brainwave.ManagementStudents.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.ManagementStudents.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task Add(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public async Task Update(Student student)
        {
            _context.Students.Update(student);
        }

        public async Task<Student?> GetById(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Enrollment?> GetEnrollmentByStudentId(Guid id)
        {
            return await _context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == id);
        }

        public async Task Add(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }

        public async Task Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
