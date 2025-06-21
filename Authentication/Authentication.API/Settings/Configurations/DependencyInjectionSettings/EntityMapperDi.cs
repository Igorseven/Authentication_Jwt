using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Mappers;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class EntityMapperDi
{
    public static IServiceCollection AddEntityMapperDi(this IServiceCollection services)
    {
        return services.AddScoped<IUserIdentityMapper, UserMapper>()
                       .AddScoped<IRoleMapper, RoleMapper>();
    }
}
