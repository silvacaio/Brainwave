using Brainwave.ManagementCourses.Application.Queries;
using Brainwave.ManagementCourses.Application.Queries.ViewModels;
using Brainwave.ManagementCourses.Domain;
using Brainwave.ManagementCourses.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Brainwave.ManagementCourses.Application.Tests.Queries
{
    public class CourseQueriesTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly CourseQueries _queries;

        public CourseQueriesTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _queries = new CourseQueries(_courseRepositoryMock.Object);
        }

        [Fact(DisplayName = "Should return null if course not found")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetById_ShouldReturnNull_WhenCourseNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            _courseRepositoryMock.Setup(r => r.GetById(courseId, true)).ReturnsAsync((Course)null!);

            // Act
            var result = await _queries.GetById(courseId);

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Should return course view model when found")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetById_ShouldReturnCourseViewModel_WhenCourseExists()
        {
            // Arrange
            var course = CreateSampleCourse();
            _courseRepositoryMock.Setup(r => r.GetById(course.Id, true)).ReturnsAsync(course);

            // Act
            var result = await _queries.GetById(course.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(course.Id, result.Id);
            Assert.Equal(course.Title, result.Title);
        }

        [Fact(DisplayName = "Should return all courses")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetAll_ShouldReturnAllCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                CreateSampleCourse(),
                CreateSampleCourse()
            };

            _courseRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(courses);

            // Act
            var result = await _queries.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "Should return courses not in provided list")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetCoursesNotIn_ShouldReturnCoursesNotInList()
        {
            // Arrange
            var courses = new List<Course>
            {
                CreateSampleCourse()
            };
            var excludedIds = new[] { Guid.NewGuid() };

            _courseRepositoryMock.Setup(r => r.GetCoursesNotIn(excludedIds)).ReturnsAsync(courses);

            // Act
            var result = await _queries.GetCoursesNotIn(excludedIds);

            // Assert
            Assert.Single(result);
        }

        [Fact(DisplayName = "Should return empty list when no courses found in GetCoursesNotIn")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetCoursesNotIn_ShouldReturnEmpty_WhenNoneFound()
        {
            // Arrange
            _courseRepositoryMock.Setup(r => r.GetCoursesNotIn(It.IsAny<Guid[]>())).ReturnsAsync(new List<Course>());

            // Act
            var result = await _queries.GetCoursesNotIn(new Guid[0]);

            // Assert
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should return lesson view model when found")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetLessonByCourseIdAndLessonId_ShouldReturnLessonViewModel_WhenLessonExists()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var lessonId = Guid.NewGuid();

            var lesson = Lesson.LessonFactory.New(courseId, "Lesson title", "Lesson content", "Material");
            typeof(Lesson).GetProperty(nameof(Lesson.Id))!.SetValue(lesson, lessonId);

            _courseRepositoryMock.Setup(r => r.GetLessonByIdAndCourseId(lessonId, courseId))
                                 .ReturnsAsync(lesson);

            // Act
            var result = await _queries.GetLessonByCourseIdAndLessonId(courseId, lessonId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lessonId, result.Id);
            Assert.Equal("Lesson title", result.Title);
        }

        [Fact(DisplayName = "Should return null when lesson not found")]
        [Trait("Course", "ManagementCourses - CourseQueries")]
        public async Task GetLessonByCourseIdAndLessonId_ShouldReturnNull_WhenLessonNotFound()
        {
            // Arrange
            _courseRepositoryMock.Setup(r => r.GetLessonByIdAndCourseId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                 .ReturnsAsync((Lesson)null!);

            // Act
            var result = await _queries.GetLessonByCourseIdAndLessonId(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        private Course CreateSampleCourse()
        {
            var course = Domain.Course.CourseFactory.New(
                "Test Course",
                100,
                new Syllabus("Syllabus Content", 10, "EN")
            );

            course.AddLesson(Lesson.LessonFactory.New(course.Id, "Lesson 1", "Content", null));

            return course;
        }
    }
}
