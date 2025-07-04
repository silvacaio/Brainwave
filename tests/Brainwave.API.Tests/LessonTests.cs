using Brainwave.API.Tests.Config;

using System.Net;
using System.Net.Http.Json;

namespace Brainwave.API.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class LessonIntegrationTests
    {
        private readonly IntegrationTestsFixture _fixture;

        public LessonIntegrationTests(IntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Add Lesson Successfully")]
        [Trait("Category", "API Integration - Lesson")]
        public async Task Add_NewLesson_ShouldSucceed()
        {
            // Arrange
            await _fixture.PerformApiLogin();
            _fixture.Client.AssignToken(_fixture.Token);

            var courseId = await _fixture.GetFirstCourseId();

            var lesson = new
            {
                Title = "Introduction to Testing",
                Content = "Learn how to write integration tests in .NET.",
                Material = "https://example.com/material",
                CourseId = courseId
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync("/api/lessons", lesson);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Add Lesson with Invalid Course")]
        [Trait("Category", "API Integration - Lesson")]
        public async Task Add_LessonToNonexistentCourse_ShouldReturnNotFound()
        {
            // Arrange
            await _fixture.PerformApiLogin();
            _fixture.Client.AssignToken(_fixture.Token);

            var lesson = new
            {
                Title = "Invalid Course",
                Content = "This course does not exist.",
                Material = "https://example.com/material",
                CourseId = Guid.NewGuid()
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync("/api/lessons", lesson);
            var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("The specified course does not exist.", errors.ToString());
        }

        [Fact(DisplayName = "Finish Lesson Successfully")]
        [Trait("Category", "API Integration - Lesson")]
        public async Task Finish_Lesson_ShouldSucceed()
        {
            // Arrange
            await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);

            var result = await _fixture.EnsureStudentHasActiveEnrollmentWithProgress();

            var lessonFinish = new
            {
                CourseId = result.CourseId,
                LessonId = result.Id
            };

            // Act
            var response = await _fixture.Client.PutAsJsonAsync("/api/lessons/finish", lessonFinish);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Finish Lesson with Invalid IDs")]
        [Trait("Category", "API Integration - Lesson")]
        public async Task Finish_NonexistentLesson_ShouldReturnNotFound()
        {
            // Arrange
            await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);

            var lessonFinish = new
            {
                CourseId = Guid.NewGuid(),
                LessonId = Guid.NewGuid()
            };

            // Act
            var response = await _fixture.Client.PutAsJsonAsync("/api/lessons/finish", lessonFinish);
            var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("The specified lesson does not exist.", errors.ToString());
        }
    }

}

