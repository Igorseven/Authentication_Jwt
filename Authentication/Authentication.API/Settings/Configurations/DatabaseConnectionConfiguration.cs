using Authentication.Domain.Providers;
using Authentication.Infrastructure.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Settings.Configurations;

public static class DatabaseConnectionConfiguration
{
    public static void AddDatabaseConnectionConfiguration(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>((serviceProv, options) =>
            options.UseSqlServer(
                serviceProv.GetRequiredService<ConnectionStringOptions>().DefaultConnection, 
                sql => sql.CommandTimeout(180)));
    }
}