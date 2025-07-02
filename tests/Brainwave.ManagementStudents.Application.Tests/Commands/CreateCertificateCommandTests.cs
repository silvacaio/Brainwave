
using Brainwave.ManagementStudents.Application.Commands.Cetificates;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class CreateCertificateCommandTests
    {
        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllFieldsAreValid()
        {
            // Arrange
            var command = new CreateCertificateCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenStudentIdIsEmpty()
        {
            // Arrange
            var command = new CreateCertificateCommand(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The StudentId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCourseIdIsEmpty()
        {
            // Arrange
            var command = new CreateCertificateCommand(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The CourseId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenEnrollmentIdIsEmpty()
        {
            // Arrange
            var command = new CreateCertificateCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The EnrollmentId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAllFieldsAreEmpty()
        {
            // Arrange
            var command = new CreateCertificateCommand(Guid.Empty, Guid.Empty, Guid.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Equal(3, command.ValidationResult.Errors.Count);
        }
    }
}
