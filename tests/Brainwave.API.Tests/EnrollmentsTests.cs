using Brainwave.API.Tests.Config;
using  Brainwave.API.Tests.Config;

namespace Brainwave.API.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class EnrollmentsTests
{
    private readonly IntegrationTestsFixture _fixture;
    public EnrollmentsTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Successfully Enroll Student")]
    [Trait("Enrollments", "API Integration - Enrollment")]
    public async Task Add_NewEnrollment_ShouldSucceed()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);

        var courseId = await _fixture.GetFirstCourseId();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/enrollments/{courseId}", null);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Enrollment With Error")]
    [Trait("Enrollments", "API Integration - Enrollment")]
    public async Task Add_NewEnrollment_ShouldReturnErrorMessages()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);

        var courseId = Guid.NewGuid();

        // Act
        var response = await _fixture.Client.PostAsync($"/api/enrollments/{courseId}", null);
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Contains("Course not found.", errors.ToString());
    }
}
