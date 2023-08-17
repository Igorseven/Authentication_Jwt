using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class AuthenticationDI
{
    public static IServiceCollection AddAuthenticationDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(config =>
       {
           config.RequireHttpsMetadata = false;
           config.SaveToken = true;
           config.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero,
               ValidAudience = configuration["JwtToken:Audience"],
               ValidIssuer = configuration["JwtToken:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:JwtKey"]!)),
           };

       })
       .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(IdentityConstants.ApplicationScheme);

        return services;
    }
}
