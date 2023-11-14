using Authentication.Domain.Entities;
using Authentication.Domain.Extensions;
using Authentication.Infrastructure.ORM.Context;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class IdentityDi
{
    public static IServiceCollection AddIdentityDi(this IServiceCollection services)
    {

        services.AddIdentityCore<UserIdentity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.User.AllowedUserNameCharacters = IdentityExtension.GetAllWritableCharacters(Encoding.UTF8);
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
        }).AddRoles<Role>()
          .AddRoleManager<RoleManager<Role>>()
          .AddSignInManager<SignInManager<UserIdentity>>()
          .AddRoleValidator<RoleValidator<Role>>()
          .AddEntityFrameworkStores<ApplicationContext>()
          .AddDefaultTokenProviders();

        return services;
    }
}

