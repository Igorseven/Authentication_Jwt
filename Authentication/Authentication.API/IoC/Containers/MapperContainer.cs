using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Mappers;

namespace Authentication.API.IoC.Containers;

public static class MapperContainer
{
    public static IServiceCollection AddMapperContainer(this IServiceCollection services)
    {
        return services.AddScoped<IUserMapper, UserMapper>()
                       .AddScoped<IRoleMapper, RoleMapper>();
    }
}
