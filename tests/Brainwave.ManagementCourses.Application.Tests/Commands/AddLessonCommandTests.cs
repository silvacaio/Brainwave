using Brainwave.ManagementCourses.Application.Commands.Lesson;
using System;
using Xunit;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class AddLessonCommandTests
    {
        [Fact(DisplayName = "Should be valid when command is filled correctly")]
        [Trait("Lesson", "ManagementCourses - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeValid_WhenCommandIsValid()
        {
            // Arrange
            var command = new AddLessonCommand(
                title: "Lesson Title",
                content: "Lesson Content",
                material: "Optional material",
                courseId: Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.True(result);
            Assert.Empty(command.ValidationResult.Errors);
        }

        [Fact(DisplayName = "Should be invalid when title is empty")]
        [Trait("Lesson", "ManagementCourses - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenTitleIsEmpty()
        {
            // Arrange
            var command = new AddLessonCommand(
                title: "",
                content: "Lesson Content",
                material: "Optional material",
                courseId: Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "Title");
        }

        [Fact(DisplayName = "Should be invalid when content is empty")]
        [Trait("Lesson", "ManagementCourses - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenContentIsEmpty()
        {
            // Arrange
            var command = new AddLessonCommand(
                title: "Lesson Title",
                content: "",
                material: "Optional material",
                courseId: Guid.NewGuid());

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "Content");
        }

        [Fact(DisplayName = "Should be invalid when courseId is empty")]
        [Trait("Lesson", "ManagementCourses - AddLessonCommand")]
        public void AddLessonCommand_ShouldBeInvalid_WhenCourseIdIsEmpty()
        {
            // Arrange
            var command = new AddLessonCommand(
                title: "Lesson Title",
                content: "Lesson Content",
                material: "Optional material",
                courseId: Guid.Empty);

            // Act
            var result = command.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(command.ValidationResult.Errors, e => e.PropertyName == "CourseId");
        }
    }
}
