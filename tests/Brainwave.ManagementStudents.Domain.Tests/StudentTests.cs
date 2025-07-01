using Brainwave.Core.DomainObjects;

namespace Brainwave.ManagementStudents.Domain.Tests
{
    public class StudentTests
    {
        [Fact]
        public void Constructor_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Student(id, "", false));
            Assert.Equal("Nome do aluno não pode ser vazio.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldCreateStudent_WhenValidName()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "João";

            // Act
            var student = new Student(id, name, false);

            // Assert
            Assert.Equal(id, student.Id);
            Assert.Equal(name, student.Name);
            Assert.False(student.IsAdmin);
            Assert.Empty(student.Enrollments);
        }

        [Fact]
        public void AddEnrollment_ShouldAdd_WhenEnrollmentIsValidAndNotExists()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var student = Student.StudentFactory.CreateStudent(studentId, "Maria");
            var courseId = Guid.NewGuid();
            var enrollment = Enrollment.EnrollmentActive.Create(studentId, courseId);

            // Act
            student.AddEnrollment(enrollment);

            // Assert
            Assert.Single(student.Enrollments);
            Assert.Equal(enrollment, student.Enrollments.First());
        }

        [Fact]
        public void AddEnrollment_ShouldThrow_WhenEnrollmentIsNull()
        {
            // Arrange
            var student = Student.StudentFactory.CreateStudent(Guid.NewGuid(), "Carlos");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => student.AddEnrollment(null!));
            Assert.Equal("Matrícula não pode ser nula.", ex.Message);
        }

        [Fact]
        public void AddEnrollment_ShouldThrow_WhenEnrollmentAlreadyExists()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var student = Student.StudentFactory.CreateStudent(studentId, "Julia");
            var courseId = Guid.NewGuid();
            var enrollment = Enrollment.EnrollmentDone.Create(studentId, courseId);
            student.AddEnrollment(enrollment);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => student.AddEnrollment(enrollment));
            Assert.Equal("Matrícula já existente.", ex.Message);
        }

        [Fact]
        public void GetEnrollment_ShouldReturnCorrectEnrollment_WhenExists()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var student = Student.StudentFactory.CreateStudent(studentId, "Pedro");
            var enrollment = Enrollment.EnrollmentPendingPayment.Create(studentId, courseId);
            student.AddEnrollment(enrollment);

            // Act
            var result = student.GetEnrollment(courseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(enrollment, result);
        }

        [Fact]
        public void GetEnrollment_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var student = Student.StudentFactory.CreateStudent(Guid.NewGuid(), "Ana");

            // Act
            var result = student.GetEnrollment(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void StudentFactory_ShouldCreateStudent_WithIsAdminFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Lucas";

            // Act
            var student = Student.StudentFactory.CreateStudent(id, name);

            // Assert
            Assert.False(student.IsAdmin);
        }

        [Fact]
        public void StudentFactory_ShouldCreateAdmin_WithIsAdminTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Admin";

            // Act
            var student = Student.StudentFactory.CreateAdmin(id, name);

            // Assert
            Assert.True(student.IsAdmin);
        }
    }
}
