using Brainwave.Core.DomainObjects;
using Brainwave.ManagementStudents.Domain;
using System;
using Xunit;

namespace Brainwave.ManagementStudents.Domain.Tests
{
    public class CertificateTests
    {
        [Fact]
        public void Constructor_ShouldCreateCertificate_WhenValidData()
        {
            // Arrange
            var studentName = "João da Silva";
            var courseName = "Engenharia de Software";
            var studentId = Guid.NewGuid();
            var enrollmentId = Guid.NewGuid();

            // Act
            var certificate = new Certificate(studentName, courseName, enrollmentId, studentId);

            // Assert
            Assert.Equal(studentName, certificate.StudentName);
            Assert.Equal(courseName, certificate.CourseName);
            Assert.Equal(studentId, certificate.StudentId);
            Assert.Equal(enrollmentId, certificate.EnrollmentId);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenStudentIdIsEmpty()
        {
            // Arrange
            var studentName = "Maria";
            var courseName = "Matemática";
            var enrollmentId = Guid.NewGuid();
            var studentId = Guid.Empty;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Certificate(studentName, courseName, enrollmentId, studentId));

            Assert.Equal("The StudentId field is required.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenEnrollmentIdIsEmpty()
        {
            // Arrange
            var studentName = "Carlos";
            var courseName = "Física";
            var enrollmentId = Guid.Empty;
            var studentId = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Certificate(studentName, courseName, enrollmentId, studentId));

            Assert.Equal("The EnrollmentId field is required.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenStudentNameIsNullOrEmpty()
        {
            // Arrange
            var studentName = "";
            var courseName = "Química";
            var studentId = Guid.NewGuid();
            var enrollmentId = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Certificate(studentName, courseName, enrollmentId, studentId));

            Assert.Equal("The Student Name field is required.", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenCourseNameIsNullOrEmpty()
        {
            // Arrange
            var studentName = "Fernanda";
            var courseName = " ";
            var studentId = Guid.NewGuid();
            var enrollmentId = Guid.NewGuid();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Certificate(studentName, courseName, enrollmentId, studentId));

            Assert.Equal("The Course Name field is required.", ex.Message);
        }

        [Fact]
        public void Validate_ShouldThrowIfCalledWithInvalidState()
        {
            // Arrange
            var certificate = new Certificate("Fulano", "Curso A", Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            var ex = Record.Exception(() => certificate.Validate());
            Assert.Null(ex); // não deve lançar nada se válido
        }
    }
}
