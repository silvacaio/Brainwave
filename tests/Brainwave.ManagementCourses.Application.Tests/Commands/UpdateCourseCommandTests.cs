using Brainwave.ManagementCourses.Application.Commands.Course;
using System;
using Xunit;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class UpdateCourseCommandTests
    {
        [Fact(DisplayName = "Should be valid when all properties are properly filled")]
        [Trait("Course", "ManagementCourses - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeValid_WhenAllPropertiesAreCorrect()
        {
            // Arrange
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "Course Title",
                "Full syllabus content",
                20,
                "English",
                150.00m
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "Should be invalid when Id is empty")]
        [Trait("Course", "ManagementCourses - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenIdIsEmpty()
        {
            // Arrange
            var command = new UpdateCourseCommand(
                Guid.Empty,
                "Course Title",
                "Syllabus",
                10,
                "PT-BR",
                100
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Invalid course id");
        }

        [Fact(DisplayName = "Should be invalid when any required property is missing or invalid")]
        [Trait("Course", "ManagementCourses - UpdateCourseCommand")]
        public void UpdateCourseCommand_ShouldBeInvalid_WhenPropertiesAreInvalid()
        {
            // Arrange
            var command = new UpdateCourseCommand(
                Guid.NewGuid(),
                "",
                "",
                0,
                "",
                0
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Title is required");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Syllabus Content is required");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Syllabus languague is required");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Syllabus duration in hours should be greater than 0");
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Value should be greater than 0");
        }
    }
}
