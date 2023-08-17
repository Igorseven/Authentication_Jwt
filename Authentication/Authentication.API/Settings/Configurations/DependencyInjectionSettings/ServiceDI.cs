using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.Services.AuthenticationServices;
using Authentication.ApplicationService.Services.RoleServices;
using Authentication.ApplicationService.Services.UserIdentityServices;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class ServiceDI
{
    public static IServiceCollection AddServiceDI(this IServiceCollection services)
    {
        return services.AddScoped<IUserIdentityCommandService, UserIdentityCommandService>()
                       .AddScoped<IUserIdentityQueryService, UserIdentityQueryService>()
                       .AddScoped<IAuthenticationCommandService, AuthenticationCommandService>()
                       .AddScoped<IRoleCommandService, RoleCommandService>()
                       .AddScoped<IRoleQueryService, RoleQueryService>();
        
    }
}
