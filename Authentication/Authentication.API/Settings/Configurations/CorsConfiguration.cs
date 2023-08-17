namespace Authentication.API.Settings.Configurations;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(p => p.AddPolicy("DfPolicy", builder =>
        {
            builder.AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed(origin => true)
                   .AllowCredentials();
        }));

        return services;
    }
}
