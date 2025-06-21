using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.Services.AuthenticationServices;
using Authentication.ApplicationService.Services.RoleServices;
using Authentication.ApplicationService.Services.UserServices;

namespace Authentication.API.IoC.Containers;

public static class ServiceContainer
{
    public static IServiceCollection AddServiceContainer(this IServiceCollection services) =>
        services.AddScoped<IUserCommandService, UserCommandService>()
            .AddScoped<IUserQueryService, UserQueryService>()
            .AddScoped<IAuthenticationCommandService, AuthenticationCommandService>()
            .AddScoped<IRoleCommandService, RoleCommandService>()
            .AddScoped<IRoleQueryService, RoleQueryService>();
}
