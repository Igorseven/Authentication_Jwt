using Authentication.API.Extensions;
using Authentication.Domain.Providers;

namespace Authentication.API.Settings.Configurations;

public static class ProviderConfiguration
{
    public static void AddProviderConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetConfigureOptions<ConnectionStringOptions>(configuration, ConnectionStringOptions.SectionName);
        services.SetConfigureOptions<JwtTokenOptions>(configuration, JwtTokenOptions.SectionName);
    }
}