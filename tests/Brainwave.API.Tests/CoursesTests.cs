using System.Net.Http.Json;
using Bogus;
using Brainwave.API.Tests.Config;
using Brainwave.API.ViewModel;

namespace Brainwave.API.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class CoursesTests
{
    private readonly IntegrationTestsFixture _fixture;
    public CoursesTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Register Course Successfully")]
    [Trait("Course", "API Integration - Course")]
    public async Task Add_NewCourse_ShouldSucceed()
    {
        var faker = new Faker("pt_BR");
        // Arrange
        var data = new CourseViewModel
        {
            Title = Guid.NewGuid().ToString(),
            SyllabusContent = "Curso de Azure",
            SyllabusDurationInHours = 50,
            SyllabusLanguage = "Portugues",
            Value = 1000
        };

        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/courses", data);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }

    [Fact(DisplayName = "Register Course With Error")]
    [Trait("Course", "API Integration - Course")]
    public async Task Add_NewCourseWithInvalidData_ShouldReturnErrorMessages()
    {
        // Arrange
        var data = new CourseViewModel
        {
            Title = "",
            SyllabusDurationInHours = 0,
            SyllabusLanguage = "",
            SyllabusContent = "",
            Value = 0
        };

        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/courses", data);
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Title is required", errors.ToString());
        Assert.Contains("Syllabus Content is required", errors.ToString());
        Assert.Contains("Syllabus languague is required", errors.ToString());
        Assert.Contains("Syllabus duration in hours should be greater than 0", errors.ToString());
        Assert.Contains("Value should be greater than 0", errors.ToString());
    }

    [Fact(DisplayName = "Update Course Successfully")]
    [Trait("Course", "API Integration - Course"), TestPriority(1)]
    public async Task Update_ExistingCourse_ShouldSucceed()
    {
        // Arrange
        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);
        var id = await _fixture.GetFirstCourseId();
        var data = new CourseViewModel
        {
            Id = id,
            Title = "Updated Curso",
            SyllabusContent = "Updated Curso Content",
            SyllabusDurationInHours = 60,
            SyllabusLanguage = "English",
            Value = 2000,
        };

        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/courses/{id}", data);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }

    [Fact(DisplayName = "Complete Course Successfully")]
    [Trait("Course", "API Integration - Course"), TestPriority(2)]
    public async Task FinishCourse_CompletedLessons_ShouldSucceed()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno1@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);

        var courseId = await _fixture.GetDotNetCourse();

        // Act
        var response = await _fixture.Client.PutAsync($"/api/courses/{courseId}/finish", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Complete Course With Error")]
    [Trait("Course", "API Integration - Course")]
    public async Task CompleteCourse_CourseWithouLessons_ShouldReturnErrorMessages()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);

        var courseId = await _fixture.GetCourseWithoutLessons();

        // Act
        var response = await _fixture.Client.PutAsync($"/api/courses/{courseId}/finish", null);
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("You have not started any lessons in this course.", errors.ToString());
    }

    [Fact(DisplayName = "Complete Course With Error")]
    [Trait("Course", "API Integration - Course")]
    public async Task CompleteCourse_UnfinishedLessons_ShouldReturnErrorMessages()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);

        var data = await _fixture.GetCourse_UnfinishedLessonsLessons();

        // Act
        var response = await _fixture.Client.PutAsync($"/api/courses/{data.CourseId}/finish", null);
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("You must complete all lessons in the course before finishing it.", errors.ToString());
    }



    [Fact(DisplayName = "Update Course With Error")]
    [Trait("Course", "API Integration - Course")]
    public async Task Update_NonExistentCourse_ShouldReturnErrorMessages()
    {
        // Arrange
        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);
        var id = Guid.NewGuid();
        var data = new CourseViewModel
        {
            Id = id,
            Title = "Updated Curso",
            SyllabusContent = "Updated Curso Content",
            SyllabusDurationInHours = 60,
            SyllabusLanguage = "English",
            Value = 2000,
        };

        // Act
        var response = await _fixture.Client.PutAsJsonAsync($"/api/courses/{id}", data);
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Course not found.", errors.ToString());
    }

    [Fact(DisplayName = "Delete Course Successfully")]
    [Trait("Course", "API Integration - Course")]
    public async Task Delete_ExistingCourse_ShouldSucceed()
    {
        // Arrange
        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);
        var courseId = await _fixture.GetCourseWithoutLessons();

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/courses/{courseId}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(!string.IsNullOrEmpty(result));
    }

    [Fact(DisplayName = "Delete Nonexistent Course")]
    [Trait("Course", "API Integration - Course")]
    public async Task Delete_NonExistentCourse_ShouldReturnErrorMessages()
    {
        // Arrange
        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);
        var id = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/courses/{id}");
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Course not found.", errors.ToString());
    }

    [Fact(DisplayName = "Delete Course With Associated Lessons")]
    [Trait("Course", "API Integration - Course")]
    public async Task Delete_CourseWithLessons_ShouldReturnErrorMessages()
    {
        // Arrange
        await _fixture.PerformApiLogin();
        _fixture.Client.AssignToken(_fixture.Token);
        var id = await _fixture.GetCourseWithLessons();

        // Act
        var response = await _fixture.Client.DeleteAsync($"/api/courses/{id}");
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Cannot delete a course that has lessons", errors.ToString());
    }
}
