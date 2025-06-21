using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.Services.AuthenticationServices;
using Authentication.ApplicationService.Services.RoleServices;
using Authentication.ApplicationService.Services.UserIdentityServices;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class ServiceDi
{
    public static IServiceCollection AddServiceDi(this IServiceCollection services)
    {
        return services.AddScoped<IUserCommandService, UserCommandService>()
                       .AddScoped<IUserQueryService, UserQueryService>()
                       .AddScoped<IAuthenticationCommandService, AuthenticationCommandService>()
                       .AddScoped<IRoleCommandService, RoleCommandService>()
                       .AddScoped<IRoleQueryService, RoleQueryService>();
        
    }
}
