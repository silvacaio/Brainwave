using Brainwave.ManagementCourses.Application.Commands.Course;
using System;
using Xunit;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class DeleteCourseCommandTests
    {
        [Fact(DisplayName = "Should be valid when course Id is provided")]
        [Trait("Course", "ManagementCourses - DeleteCourseCommand")]
        public void DeleteCourseCommand_ShouldBeValid_WhenIdIsProvided()
        {
            // Arrange
            var command = new DeleteCourseCommand(Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "Should be invalid when course Id is empty")]
        [Trait("Course", "ManagementCourses - DeleteCourseCommand")]
        public void DeleteCourseCommand_ShouldBeInvalid_WhenIdIsEmpty()
        {
            // Arrange
            var command = new DeleteCourseCommand(Guid.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Course id is required");
        }
    }
}
