using Authentication.API.IoC.Containers;
using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.ORM.UoW;

namespace Authentication.API.IoC;

public static class InversionOfControlHandler
{
    public static void AddInversionOfControlHandler(this IServiceCollection services) =>
        services.AddScoped<ApplicationContext>()
            .AddScoped<INotificationHandler, NotificationHandler>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddValidationContainer()
            .AddRepositoryContainer()
            .AddMapperContainer()
            .AddServiceContainer();
}