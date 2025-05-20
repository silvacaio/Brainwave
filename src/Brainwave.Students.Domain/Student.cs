using Brainwave.Core.DomainObjects;

namespace Brainwave.Students.Domain
{
    public class Student : Entity, IAggregateRoot
    {
        private readonly List<Enrollment> _enrollments = [];
        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments;

        public void AddEnrollment(Enrollment Enrollment)
        {
            if (EnrollmentExists(Enrollment))
                throw new DomainException("Matrícula já existente.");

            //TOOD : Verificar se o aluno já está matriculado no curso
            //TODO:  Pagamento pendente

            _enrollments.Add(Enrollment);
        }

        public Enrollment? GetEnrollment(Guid cursoId)
        {
            return Enrollments.FirstOrDefault(m => m.CourseId == cursoId && m.StudentId == Id);
        }

        private bool EnrollmentExists(Enrollment Enrollment)
        {
            return Enrollments.Any(m => m.Id == Enrollment.Id);
        }
    }
}
