using Brainwave.Core.DomainObjects;
using System;
using Xunit;

namespace Brainwave.ManagementCourses.Domain.Tests
{
    public class LessonTests
    {
        [Fact]
        public void Constructor_ShouldCreateValidLesson()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var title = "Introdução ao C#";
            var content = "Aprenda C# básico.";
            var material = "video.mp4";

            // Act
            var lesson = new Lesson(courseId, title, content, material);

            // Assert
            Assert.Equal(courseId, lesson.CourseId);
            Assert.Equal(title, lesson.Title);
            Assert.Equal(content, lesson.Content);
            Assert.Equal(material, lesson.Material);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrow_WhenTitleIsEmpty(string? title)
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var content = "Conteúdo válido";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Lesson(courseId, title!, content, null));

            Assert.Equal("Title is required", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrow_WhenContentIsEmpty(string? content)
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var title = "Título válido";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Lesson(courseId, title, content!, null));

            Assert.Equal("Content is required", ex.Message);
        }

        [Fact]
        public void AssociateCourse_ShouldChangeCourseId()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var lesson = new Lesson(courseId, "Título", "Conteúdo", null);
            var newCourseId = Guid.NewGuid();

            // Act
            lesson.AssociateCourse(newCourseId);

            // Assert
            Assert.Equal(newCourseId, lesson.CourseId);
        }

        [Fact]
        public void LessonFactory_ShouldCreateValidLesson()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            // Act
            var lesson = Lesson.LessonFactory.New(courseId, "Título", "Conteúdo", "arquivo.pdf");

            // Assert
            Assert.Equal("Título", lesson.Title);
            Assert.Equal("Conteúdo", lesson.Content);
            Assert.Equal("arquivo.pdf", lesson.Material);
        }      
    }
}
