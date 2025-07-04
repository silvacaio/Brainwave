using Brainwave.API.Tests.Config;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace Brainwave.API.Tests;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class StudentTests
{
    private readonly IntegrationTestsFixture _fixture;

    public StudentTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }
}
