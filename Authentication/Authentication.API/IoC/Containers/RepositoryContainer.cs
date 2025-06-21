using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.Repository;

namespace Authentication.API.IoC.Containers;

public static class RepositoryContainer
{
    public static IServiceCollection AddRepositoryContainer(this IServiceCollection services)
    {
        return services.AddScoped<IUserRepository, UserRepository>()
                        .AddScoped<IRoleRepository, RoleRepository>()
                        .AddScoped<IUserRoleRepository, UserRoleRepository>()
                        .AddScoped<IUserTokenRepository, UserTokenRepository>();
    }
}
