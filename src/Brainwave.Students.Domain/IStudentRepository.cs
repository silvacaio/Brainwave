using Brainwave.Core.Data;

namespace Brainwave.Students.Domain
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetById(Guid id);

        Task<Enrollment?> GetEnrollmentByStudentId(Guid id);
    }
}
