using Microsoft.Extensions.DependencyInjection;

namespace Authentication.IntegrationTest.Settings;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient _client;
    protected readonly IServiceScope _scope;
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
    }
}