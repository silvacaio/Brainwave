using Brainwave.Core.Data;

namespace Brainwave.Students.Domain
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task Add(Student student);
        Task Update(Student student);

        Task<Student?> GetById(Guid id);

        Task<Enrollment?> GetEnrollmentByStudentId(Guid id);
        Task Add(Enrollment enrollment);
        Task Update(Enrollment enrollment);
    }
}
