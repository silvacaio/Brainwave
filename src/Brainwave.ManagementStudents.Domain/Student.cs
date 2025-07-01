using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementStudents.Domain
{
    public class Student : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsAdmin { get; private set; }

        private readonly List<Enrollment> _enrollments = [];

        public Student(Guid id, string name, bool isAdmin)
        {
            Id = id;
            Name = name;
            IsAdmin = isAdmin;

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Nome do aluno não pode ser vazio.");

        }

        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments;

        public void AddEnrollment(Enrollment Enrollment)
        {
            if (Enrollment == null)
                throw new DomainException("Matrícula não pode ser nula.");

            if (EnrollmentExists(Enrollment))
                throw new DomainException("Matrícula já existente.");

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

        public static class StudentFactory
        {
            public static Student CreateStudent(Guid id, string name)
            {
                return new Student(id, name, false);
            }

            public static Student CreateAdmin(Guid id, string name)
            {
                return new Student(id, name, true);
            }
        }
    }
}
