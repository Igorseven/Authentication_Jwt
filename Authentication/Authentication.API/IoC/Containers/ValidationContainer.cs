using Authentication.Domain.Entities;
using Authentication.Domain.EntitiesValidations;
using Authentication.Domain.Interfaces.OthersContracts;

namespace Authentication.API.IoC.Containers;

public static class ValidationContainer
{
    public static IServiceCollection AddValidationContainer(this IServiceCollection services) =>
        services.AddScoped<IValidate<User>, UserValidation>()
            .AddScoped<IValidate<Role>, RoleValidation>();
}
