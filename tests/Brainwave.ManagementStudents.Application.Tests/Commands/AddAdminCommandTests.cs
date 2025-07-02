using System;
using Xunit;
using Brainwave.ManagementStudents.Application.Commands.User;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class AddAdminCommandTests
    {
        [Fact(DisplayName = "Should be valid when all fields are filled")]
        [Trait("AddAdminCommand", "Validation")]
        public void AddAdminCommand_ShouldBeValid_WhenFieldsAreCorrect()
        {
            // Arrange
            var command = new AddAdminCommand(Guid.NewGuid(), "Admin Name");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when UserId is empty")]
        [Trait("AddAdminCommand", "Validation")]
        public void AddAdminCommand_ShouldBeInvalid_WhenUserIdIsEmpty()
        {
            // Arrange
            var command = new AddAdminCommand(Guid.Empty, "Admin Name");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The UserId field is required.");
        }

        [Fact(DisplayName = "Should be invalid when Name is empty")]
        [Trait("AddAdminCommand", "Validation")]
        public void AddAdminCommand_ShouldBeInvalid_WhenNameIsEmpty()
        {
            // Arrange
            var command = new AddAdminCommand(Guid.NewGuid(), string.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The name field is required.");
        }

        [Fact(DisplayName = "Should be invalid when both fields are empty")]
        [Trait("AddAdminCommand", "Validation")]
        public void AddAdminCommand_ShouldBeInvalid_WhenAllFieldsAreEmpty()
        {
            // Arrange
            var command = new AddAdminCommand(Guid.Empty, "");

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The UserId field is required.");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The name field is required.");
        }
    }
}
