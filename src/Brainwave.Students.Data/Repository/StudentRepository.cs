using Brainwave.Core.Data;
using Brainwave.Students.Domain;
using Microsoft.EntityFrameworkCore;

namespace Brainwave.Students.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Student student)
        {
            _context.Students.Add(student);
        }

        public void Update(Student student)
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

        public void Add(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
        }

        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
