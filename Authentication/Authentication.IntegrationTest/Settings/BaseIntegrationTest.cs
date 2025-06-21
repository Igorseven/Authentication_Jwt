using Microsoft.Extensions.DependencyInjection;

namespace Authentication.IntegrationTest.Settings;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly IServiceScope Scope;
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();
        Client = factory.CreateClient();
    }
}