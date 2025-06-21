using Authentication.API.Settings.Configurations;

namespace Authentication.API.Settings;

public static class SettingsHandler
{
    public static void AddSettingsControl(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProviderConfiguration(configuration);
        services.AddControllersConfiguration();
        services.AddCorsConfiguration();
        services.AddDatabaseConnectionConfiguration();
        services.AddIdentityConfiguration();
        services.AddAuthenticationConfiguration(configuration);
        services.AddFiltersConfiguration();
        services.AddSwaggerConfiguration();
        services.AddRateLimitingConfiguration();
    }
}
