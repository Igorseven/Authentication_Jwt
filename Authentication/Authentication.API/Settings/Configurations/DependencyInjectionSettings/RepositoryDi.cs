using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.Repository;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class RepositoryDi
{
    public static IServiceCollection AddRepositoryDi(this IServiceCollection services)
    {
        return services.AddScoped<IUserIdentityRepository, UserIdentityRepository>()
                        .AddScoped<IRoleRepository, RoleRepository>()
                        .AddScoped<IUserRoleRepository, UserRoleRepository>()
                        .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}
