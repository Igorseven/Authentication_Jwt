using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Interfaces.OthersContracts;

namespace Authentication.ApplicationService.Services.Base;
public class BaseService<T> where T : class
{
    protected readonly INotificationHandler _notification;
    protected readonly IValidate<T> _validate;

    protected BaseService(INotificationHandler notification, IValidate<T> validate)
    {
        _notification = notification;
        _validate = validate;
    }

    protected async Task<bool> EntityValidationAsync(T entity)
    {
        if (_validate is null)
            return _notification.CreateNotification("Validação", "objeto inválido");

        var validationResponse = await _validate.ValidationAsync(entity);

        if (!validationResponse.Valid)
            _notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));

        return validationResponse.Valid;
    }

    protected bool EntitiesValidation(List<T> entities)
    {
        if (_validate is null)
            return _notification.CreateNotification("Validação", "objeto inválido");

        foreach (var entity in entities)
        {
            var validationResponse = _validate.Validation(entity);

            if (!validationResponse.Valid)
                _notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));
        }

        return !_notification.HasNotification();
    }
}

