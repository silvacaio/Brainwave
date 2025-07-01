using System;
using Xunit;
using Brainwave.ManagementStudents.Application.Commands.Enrollment;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class EnrollmentPaidCommandTests
    {
        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllFieldsAreValid()
        {
            // Arrange
            var command = new EnrollmentPaidCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                150.00m
            );

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenUserIdIsEmpty()
        {
            // Arrange
            var command = new EnrollmentPaidCommand(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                150.00m
            );

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The UserId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenEnrollmentIdIsEmpty()
        {
            // Arrange
            var command = new EnrollmentPaidCommand(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                150.00m
            );

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The EnrollmentId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenPaymentIdIsEmpty()
        {
            // Arrange
            var command = new EnrollmentPaidCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                150.00m
            );

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The PaymentId field cannot be empty.");
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAllGuidsAreEmpty()
        {
            // Arrange
            var command = new EnrollmentPaidCommand(
                Guid.Empty,
                Guid.Empty,
                Guid.Empty,
                150.00m
            );

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Equal(3, command.ValidationResult.Errors.Count);
        }
    }
}
