using Authentication.Infrastructure.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Settings.Configurations;

public static class MigrationHandlerConfiguration
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        
        using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
        appContext.Database.Migrate();

        return webApp;
    }
}
