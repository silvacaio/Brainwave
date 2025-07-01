using Brainwave.Core.DomainObjects;
using Brainwave.ManagementCourses.Domain.ValueObjects;


namespace Brainwave.ManagementCourses.Domain.Tests
{
    public class CourseTests
    {
        [Fact]
        public void Constructor_ShouldCreateValidCourse()
        {
            // Arrange
            var syllabus = new Syllabus("Intro", 10, "EN");

            // Act
            var course = new Course("C# Basics", 199.99m, syllabus);

            // Assert
            Assert.Equal("C# Basics", course.Title);
            Assert.Equal(199.99m, course.Value);
            Assert.Equal(syllabus, course.Syllabus);
            Assert.Empty(course.Lessons);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var syllabus = new Syllabus("Intro", 10, "EN");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Course("", 100, syllabus));

            Assert.Equal("Title is required", ex.Message);
        }
      

        [Fact]
        public void AddLesson_ShouldAdd_WhenLessonIsValid()
        {
            // Arrange
            var syllabus = new Syllabus("Intro", 10, "EN");
            var course = new Course("C# Básico", 149, syllabus);

            var lesson = new Lesson(Guid.NewGuid(), "Lesson 1", "Some content", "material.pdf");

            // Act
            course.AddLesson(lesson);

            // Assert
            Assert.Single(course.Lessons);
            Assert.Equal(lesson, course.Lessons.First());
            Assert.Equal(course.Id, lesson.CourseId);
        }

        [Fact]
        public void CourseFactory_New_ShouldReturnValidInstance()
        {
            // Arrange
            var syllabus = new Syllabus("Full stack", 50, "EN");

            // Act
            var course = Course.CourseFactory.New("Full stack dev", 399, syllabus);

            // Assert
            Assert.Equal("Full stack dev", course.Title);
            Assert.Equal(399, course.Value);
            Assert.Equal(syllabus, course.Syllabus);
        }

        [Fact]
        public void CourseFactory_Update_ShouldReturnValidInstanceWithId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var syllabus = new Syllabus("Data science", 60, "EN");

            // Act
            var course = Course.CourseFactory.Update(id, "Data", 300, syllabus);

            // Assert
            Assert.Equal(id, course.Id);
            Assert.Equal("Data", course.Title);
            Assert.Equal(syllabus, course.Syllabus);
        }
    }
}
