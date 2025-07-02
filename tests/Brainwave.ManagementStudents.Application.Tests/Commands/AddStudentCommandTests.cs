using System;
using Xunit;
using Brainwave.ManagementStudents.Application.Commands.User;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class AddStudentCommandTests
    {
        [Fact(DisplayName = "Should be valid when all fields are filled")]
        [Trait("AddStudentCommand", "Validation")]
        public void AddStudentCommand_ShouldBeValid_WhenFieldsAreCorrect()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "Student Name");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when UserId is empty")]
        [Trait("AddStudentCommand", "Validation")]
        public void AddStudentCommand_ShouldBeInvalid_WhenUserIdIsEmpty()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.Empty, "Student Name");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The UserId field is required.");
        }

        [Fact(DisplayName = "Should be invalid when Name is empty")]
        [Trait("AddStudentCommand", "Validation")]
        public void AddStudentCommand_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The name field is required.");
        }

        [Fact(DisplayName = "Should be invalid when all fields are empty")]
        [Trait("AddStudentCommand", "Validation")]
        public void AddStudentCommand_ShouldBeInvalid_WhenAllFieldsAreEmpty()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.Empty, "");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The UserId field is required.");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The name field is required.");
        }
    }
}
