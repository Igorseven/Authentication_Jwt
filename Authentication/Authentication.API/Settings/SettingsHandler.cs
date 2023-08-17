using Authentication.API.Settings.Configurations;

namespace Authentication.API.Settings;

public static class SettingsHandler
{
    public static IServiceCollection AddSettingsConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddControllersConfiguration()
                       .AddFiltersConfiguration()
                       .AddCorsConfiguration()
                       .AddSwaggerConfiguration()
                       .AddDependencyInjectionConfiguration(configuration);
    }
}
