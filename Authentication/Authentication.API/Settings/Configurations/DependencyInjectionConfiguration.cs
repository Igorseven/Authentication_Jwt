using Authentication.API.Settings.Configurations.DependencyInjectionSettings;
using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Providers;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.ORM.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.API.Settings.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(sp => sp.GetService<IOptionsMonitor<ConnectionStringOptions>>()!.CurrentValue);
        services.Configure<ConnectionStringOptions>(configuration.GetSection(ConnectionStringOptions.SectionName));

        services.AddScoped(sp => sp.GetService<IOptionsMonitor<JwtTokenOptions>>()!.CurrentValue);
        services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.SectionName));

        services.AddScoped<ApplicationContext>()
                .AddScoped<INotificationHandler, NotificationHandler>()
                .AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddDbContext<ApplicationContext>((serviceProv, options) =>
           options.UseSqlServer(serviceProv.GetRequiredService<ConnectionStringOptions>().DefaultConnection, sql => sql.CommandTimeout(180)));


        services.AddValidationDI()
                .AddRepositoryDI()
                .AddServiceDI()
                .AddEntityMapperDI()
                .AddIdentityDI()
                .AddAuthenticationDI(configuration);

        return services;
    }
}
