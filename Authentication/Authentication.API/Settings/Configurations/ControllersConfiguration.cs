using System.Text.Json.Serialization;

namespace Authentication.API.Settings.Configurations;

public static class ControllersConfiguration
{
    public static IServiceCollection AddControllersConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}
