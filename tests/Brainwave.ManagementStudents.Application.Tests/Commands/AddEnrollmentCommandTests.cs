using Brainwave.ManagementStudents.Application.Commands.Enrollment;
using System;
using Xunit;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class AddEnrollmentCommandTests
    {
        [Fact]
        public void IsValid_ShouldReturnTrue_WhenStudentIdAndCourseIdAreValid()
        {
            // Arrange
            var command = new AddEnrollmentCommand(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenStudentIdIsEmpty()
        {
            // Arrange
            var command = new AddEnrollmentCommand(Guid.Empty, Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The StudentId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCourseIdIsEmpty()
        {
            // Arrange
            var command = new AddEnrollmentCommand(Guid.NewGuid(), Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The CourseId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenBothIdsAreEmpty()
        {
            // Arrange
            var command = new AddEnrollmentCommand(Guid.Empty, Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Equal(2, command.ValidationResult.Errors.Count);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The StudentId field cannot be empty.");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The CourseId field cannot be empty.");
        }
    }
}
