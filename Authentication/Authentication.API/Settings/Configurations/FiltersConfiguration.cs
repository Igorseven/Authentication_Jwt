using Authentication.API.Filters;

namespace Authentication.API.Settings.Configurations;

public static class FiltersConfiguration
{
    public static IServiceCollection AddFiltersConfiguration(this IServiceCollection services)
    {
        services.AddMvc(config => config.Filters.AddService<NotificationFilter>());
        services.AddMvc(config => config.Filters.AddService<UnitOfWorkFilter>());

        services.AddScoped<NotificationFilter>();
        services.AddScoped<UnitOfWorkFilter>();

        return services;
    }
}
