using System;
using Xunit;
using global::Brainwave.ManagementStudents.Application.Commands.StudentLesson;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class FinishLessonCommandTests
    {
        [Fact(DisplayName = "Should be valid when all fields are properly filled")]
        [Trait("Lesson", "ManagementStudents - FinishLessonCommand")]
        public void FinishLessonCommand_ShouldBeValid_WhenAllFieldsAreCorrect()
        {
            // Arrange
            var command = new FinishLessonCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "Should be invalid when StudentId is empty")]
        [Trait("Lesson", "ManagementStudents - FinishLessonCommand")]
        public void FinishLessonCommand_ShouldBeInvalid_WhenStudentIdIsEmpty()
        {
            // Arrange
            var command = new FinishLessonCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The StudentId field cannot be empty.");
        }

        [Fact(DisplayName = "Should be invalid when CourseId is empty")]
        [Trait("Lesson", "ManagementStudents - FinishLessonCommand")]
        public void FinishLessonCommand_ShouldBeInvalid_WhenCourseIdIsEmpty()
        {
            // Arrange
            var command = new FinishLessonCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid());

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The CourseId field cannot be empty.");
        }

        [Fact(DisplayName = "Should be invalid when LessonId is empty")]
        [Trait("Lesson", "ManagementStudents - FinishLessonCommand")]
        public void FinishLessonCommand_ShouldBeInvalid_WhenLessonIdIsEmpty()
        {
            // Arrange
            var command = new FinishLessonCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty);

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "The LessonId field cannot be empty.");
        }
    }
}
