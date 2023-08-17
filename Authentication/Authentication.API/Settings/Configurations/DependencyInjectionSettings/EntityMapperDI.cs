using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Mappers;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class EntityMapperDI
{
    public static IServiceCollection AddEntityMapperDI(this IServiceCollection services)
    {
        return services.AddScoped<IUserIdentityMapper, UserIdentityMapper>()
                       .AddScoped<IRoleMapper, RoleMapper>();
    }
}
