using System.Net.Http.Json;
using Brainwave.API.Tests.Config;
using Brainwave.API.ViewModel;
using PlataformaEducacao.Api.Tests.Config;

namespace PlataformaEducacao.Api.Tests;

[TestCaseOrderer("Brainwave.API.Tests.Config.PriorityOrderer", "Brainwave.API.Tests")]
[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class UserTests
{
    private readonly IntegrationTestsFixture _fixture;

    public UserTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "Successful Login"), TestPriority(2)]
    [Trait("Category", "API Integration - Student")]
    public async Task Student_Login_ShouldSucceed()
    {
        // Arrange
        var data = new LoginUserViewModel()
        {
            Email = _fixture.UserEmail,
            Password = _fixture.UserPassword,
        };
        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/account/login", data);
        response.EnsureSuccessStatusCode();

        _fixture.SaveUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Successful Login")]
    [Trait("Category", "API Integration - Admin")]
    public async Task Admin_Login_ShouldSucceed()
    {
        // Arrange
        var data = new LoginUserViewModel()
        {
            Email = _fixture.UserEmail,
            Password = _fixture.UserPassword,
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/account/login", data);
        response.EnsureSuccessStatusCode();

        _fixture.SaveUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Login Failure")]
    [Trait("Category", "API Integration - User")]
    public async Task Login_NonExistentUser_ShouldReturnErrorMessages()
    {
        // Arrange
        var data = new LoginUserViewModel()
        {
            Email = "email@email.com",
            Password = "Teste@123",
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/account/login", data);

        // Assert
        var errors = _fixture.GetErrors(await response.Content.ReadAsStringAsync());
        Assert.Contains("Invalid username or password.", errors.ToString());
    }

    [Fact(DisplayName = "Successful Registration"), TestPriority(1)]
    [Trait("Category", "API Integration - Student")]
    public async Task Student_Registration_ShouldSucceed()
    {
        // Arrange
        _fixture.GenerateUserData();
        var register = new RegisterUserViewModel
        {
            Email = _fixture.UserEmail,
            Name = _fixture.UserEmail,
            Password = _fixture.UserPassword,
            ConfirmPassword = _fixture.PasswordConfirmation
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/account/register/student", register);
        response.EnsureSuccessStatusCode();

        _fixture.SaveUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    [Fact(DisplayName = "Successful Registration")]
    [Trait("Category", "API Integration - Admin")]
    public async Task Admin_Registration_ShouldSucceed()
    {
        // Arrange
        _fixture.GenerateUserData();
        var register = new RegisterUserViewModel
        {
            Email = _fixture.UserEmail,
            Name = _fixture.UserEmail,
            Password = _fixture.UserPassword,
            ConfirmPassword = _fixture.PasswordConfirmation
        };

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/account/register/admin", register);
        response.EnsureSuccessStatusCode();

        _fixture.SaveUserToken(await response.Content.ReadAsStringAsync());
        // Assert
        Assert.True(!string.IsNullOrEmpty(_fixture.Token));
    }

    //[Fact(DisplayName = "Generate Certificate Successfully")]
    //[Trait("Category", "API Integration - Certificate")]
    //public async Task DownloadCertificate_StudentCompletedCourse_ShouldSucceed()
    //{
    //    // Arrange
    //    await _fixture.PerformApiLogin("aluno@teste.com", "Teste@123");
    //    _fixture.Client.AssignToken(_fixture.Token);

    //    await _fixture.GetCertificateId();
    //    // Act
    //    var response = await _fixture.Client.GetAsync($"/api/alunos/certificados/{_fixture.CertificadoId}/download");
    //    response.EnsureSuccessStatusCode();

    //    // Assert
    //    var file = await response.Content.ReadAsByteArrayAsync();
    //    var filePath = Path.Combine("C:\\Temp", "certificado_teste_integracao.pdf");

    //    if (File.Exists(filePath))
    //    {
    //        File.Delete(filePath);
    //    }

    //    File.WriteAllBytes(filePath, file);
    //    Assert.True(File.Exists(filePath));
    //}

    //[Fact(DisplayName = "Get Student Learning History - With History")]
    //[Trait("Category", "API Integration - Student")]
    //public async Task GetLearningHistory_StudentHasHistory_ShouldReturnSuccessfully()
    //{
    //    // Arrange
    //    await _fixture.PerformApiLogin("aluno@teste.com", "Teste@123");
    //    _fixture.Client.AssignToken(_fixture.Token);

    //    await _fixture.GetCourseLearningHistory();
    //    // Act
    //    var response = await _fixture.Client.GetAsync($"/api/alunos/historico-aprendizagem/{_fixture.CursoId}");

    //    // Assert
    //    response.EnsureSuccessStatusCode();
    //    Assert.True(response.IsSuccessStatusCode);
    //}

    //[Fact(DisplayName = "Get Student Learning History - Without History")]
    //[Trait("Category", "API Integration - Student")]
    //public async Task GetLearningHistory_StudentNoHistory_ShouldReturnError()
    //{
    //    // Arrange
    //    await _fixture.PerformApiLogin("aluno@teste.com", "Teste@123");
    //    _fixture.Client.AssignToken(_fixture.Token);
    //    _fixture.CursoId = Guid.NewGuid();

    //    // Act
    //    var response = await _fixture.Client.GetAsync($"/api/alunos/historico-aprendizagem/{_fixture.CursoId}");

    //    // Assert
    //    Assert.False(response.IsSuccessStatusCode);
    //}
}