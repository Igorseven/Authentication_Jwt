using Authentication.Domain.Entities;
using Authentication.Domain.EntitiesValidations;
using Authentication.Domain.Interfaces.OthersContracts;

namespace Authentication.API.Settings.Configurations.DependencyInjectionSettings;

public static class ValidationDi
{
    public static IServiceCollection AddValidationDI(this IServiceCollection services)
    {
        services.AddScoped<IValidate<UserIdentity>, UserIdentityValidation>()
                .AddScoped<IValidate<Role>, RoleValidation>();

        return services;
    }
}
