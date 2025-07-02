using Brainwave.ManagementCourses.Application.Commands.Course;
using System;
using Xunit;

namespace Brainwave.ManagementCourses.Application.Tests.Commands
{
    public class AddCourseCommandTests
    {
        [Fact(DisplayName = "Should be valid when all data is correct")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeValid_WhenDataIsCorrect()
        {
            // Arrange
            var command = new AddCourseCommand(
                title: "Introduction to AI",
                syllabusContent: "Basics of AI, ML, and DL",
                syllabusDurationInHours: 40,
                syllabusLanguage: "English",
                value: 500,
                userId: Guid.NewGuid()
            );

            // Act
            var isValid = command.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact(DisplayName = "Should be invalid when title is empty")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenTitleIsEmpty()
        {
            var command = new AddCourseCommand(
                title: "",
                syllabusContent: "Content",
                syllabusDurationInHours: 10,
                syllabusLanguage: "English",
                value: 100,
                userId: Guid.NewGuid()
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Title is required");
        }

        [Fact(DisplayName = "Should be invalid when duration is 0")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenDurationIsZero()
        {
            var command = new AddCourseCommand(
                title: "Title",
                syllabusContent: "Content",
                syllabusDurationInHours: 0,
                syllabusLanguage: "English",
                value: 100,
                userId: Guid.NewGuid()
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Syllabus duration in hours should be greater than 0");
        }

        [Fact(DisplayName = "Should be invalid when value is zero")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenValueIsZero()
        {
            var command = new AddCourseCommand(
                title: "Title",
                syllabusContent: "Content",
                syllabusDurationInHours: 10,
                syllabusLanguage: "English",
                value: 0,
                userId: Guid.NewGuid()
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Value should be greater than 0");
        }

        [Fact(DisplayName = "Should be invalid when UserId is empty")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenUserIdIsEmpty()
        {
            var command = new AddCourseCommand(
                title: "Title",
                syllabusContent: "Content",
                syllabusDurationInHours: 10,
                syllabusLanguage: "English",
                value: 100,
                userId: Guid.Empty
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Invalid user");
        }

        [Fact(DisplayName = "Should be invalid when language is missing")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenLanguageIsMissing()
        {
            var command = new AddCourseCommand(
                title: "Title",
                syllabusContent: "Content",
                syllabusDurationInHours: 10,
                syllabusLanguage: "",
                value: 100,
                userId: Guid.NewGuid()
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Syllabus languague is required");
        }

        [Fact(DisplayName = "Should be invalid when syllabus content is empty")]
        [Trait("Course", "ManagementCourses - AddCourseCommand")]
        public void AddCourseCommand_ShouldBeInvalid_WhenSyllabusContentIsEmpty()
        {
            var command = new AddCourseCommand(
                title: "Title",
                syllabusContent: "",
                syllabusDurationInHours: 10,
                syllabusLanguage: "English",
                value: 100,
                userId: Guid.NewGuid()
            );

            var isValid = command.IsValid();

            Assert.False(isValid);
            Assert.Contains(command.ValidationResult.Errors, e => e.ErrorMessage == "Syllabus Content is required");
        }
    }
}
