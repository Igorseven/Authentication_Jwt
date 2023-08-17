using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.Repository;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class RepositoryDI
{
    public static IServiceCollection AddRepositoryDI(this IServiceCollection services)
    {
        return services.AddScoped<IUserIdentityRepository, UserIdentityRepository>()
                        .AddScoped<IRoleRepository, RoleRepository>()
                        .AddScoped<IUserRoleRepository, UserRoleRepository>()
                        .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
