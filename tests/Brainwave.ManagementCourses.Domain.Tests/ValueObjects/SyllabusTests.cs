using Brainwave.Core.DomainObjects;
using Brainwave.ManagementCourses.Domain.ValueObjects;

namespace Brainwave.ManagementCourses.Domain.Tests.ValueObjects
{
    public class SyllabusTests
    {
        [Fact]
        public void Constructor_ShouldCreateSyllabus_WhenValid()
        {
            // Arrange
            var content = "Algoritmos e Estruturas de Dados";
            var duration = 40;
            var language = "Português";

            // Act
            var syllabus = new Syllabus(content, duration, language);

            // Assert
            Assert.Equal(content, syllabus.Content);
            Assert.Equal(duration, syllabus.DurationInHours);
            Assert.Equal(language, syllabus.Language);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Constructor_ShouldThrow_WhenContentIsEmpty(string? content)
        {
            // Arrange
            var duration = 10;
            var language = "Português";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Syllabus(content!, duration, language));

            Assert.Equal("Content is required", ex.Message);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenDurationIsZeroOrNegative()
        {
            // Arrange
            var content = "Lógica de programação";
            var duration = 0;
            var language = "Português";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Syllabus(content, duration, language));

            Assert.Equal("Duration in hours should be greater than 0", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_ShouldThrow_WhenLanguageIsEmpty(string? language)
        {
            // Arrange
            var content = "Redes de Computadores";
            var duration = 30;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Syllabus(content, duration, language!));

            Assert.Equal("Language is required", ex.Message);
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var syllabus = new Syllabus("Banco de Dados", 20, "Português");

            // Act
            var result = syllabus.ToString();

            // Assert
            Assert.Equal("Content: Banco de Dados. Durantion in hours: 20. Language: Português", result);
        }
    }
}
