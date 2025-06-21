using System.Globalization;
using Authentication.API.Middleware;
using Authentication.API.Settings.Constants;
using Authentication.Domain.Providers;
using Microsoft.AspNetCore.Localization;

namespace Authentication.API.Settings;

public static class EcosystemInitiation
{
    public static void AddWebApplication(this WebApplication app, IConfiguration configuration)
    {
        var environmentConfiguration = LoadEnvironmentConfiguration(configuration);

        ConfigureLocalization(app);
        ConfigureSwagger(app, environmentConfiguration);
        ConfigureMiddlewares(app);
        ConfigureEndpoints(app);
    }

    private static EnvironmentConfigurationOptions LoadEnvironmentConfiguration(IConfiguration configuration) =>
        configuration.GetSection(EnvironmentConfigurationOptions.SectionName).Get<EnvironmentConfigurationOptions>()!;

    private static void ConfigureLocalization(WebApplication app)
    {
        var cultureSetup = new[] { new CultureInfo("pt-BR") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
            SupportedCultures = cultureSetup,
            SupportedUICultures = cultureSetup
        });
    }
    
    private static void ConfigureSwagger(WebApplication app, EnvironmentConfigurationOptions config)
    {
        if (!config.Active) return;

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    private static void ConfigureMiddlewares(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseMiddleware<GlobalNotificationHandlingMiddleware>();
        }

        app.UseMiddleware<CaptureStatusCodeTooManyRequestsMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors(CorsName.CorsPolicy);
        app.UseWebSockets();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapControllers();
    }
}