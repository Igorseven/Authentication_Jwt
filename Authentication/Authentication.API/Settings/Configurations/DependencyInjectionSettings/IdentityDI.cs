using Authentication.Domain.Entities;
using Authentication.Domain.Extensions;
using Authentication.Infrastructure.ORM.Context;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class IdentityDI
{
    public static IServiceCollection AddIdentityDI(this IServiceCollection services)
    {

        services.AddIdentityCore<UserIdentity>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
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

