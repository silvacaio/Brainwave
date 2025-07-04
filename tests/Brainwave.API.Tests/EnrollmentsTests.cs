using Brainwave.API.Tests.Config;
using  Brainwave.API.Tests.Config;
using System.Net;

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

        var courseId = await _fixture.GetCourseWithoutEnrollment();

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

    [Fact(DisplayName = "Download de certificado com sucesso")]
    [Trait("Categoria", "Integração API - Aluno / Certificados")]
    public async Task DownloadCertificate_DeveRetornarArquivoPdf_ComSucesso()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno1@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);

        var id = await _fixture.GetEnrollmentWithCertificate(_fixture.StudentId); // método que simula/insere certificado no banco
        var url = $"/api/enrollments/{id}/certificates/download";

        // Act
        var response = await _fixture.Client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "Download de certificado inexistente deve retornar BadRequest")]
    [Trait("Categoria", "Integração API - Aluno / Certificados")]
    public async Task DownloadCertificate_CertificateNotExists_DeveRetornarBadRequest()
    {
        // Arrange
        await _fixture.PerformApiLogin("aluno1@brainwave.com", "Teste@123");
        _fixture.Client.AssignToken(_fixture.Token);


        var newId = Guid.NewGuid();
        var url = $"/api/enrollments/{newId}/certificates/download";


        // Act
        var response = await _fixture.Client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
