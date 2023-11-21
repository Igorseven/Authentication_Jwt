using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Interfaces.OthersContracts;

namespace Authentication.ApplicationService.Services;
public abstract class BaseService<T> where T : class
{
    protected readonly INotificationHandler _notification;
    private readonly IValidate<T> _validate;

    protected BaseService(INotificationHandler notification, IValidate<T> validate)
    {
        _notification = notification;
        _validate = validate;
    }

    protected async Task<bool> EntityValidationAsync(T entity)
    {
        var validationResponse = await _validate.ValidationAsync(entity);

        if (!validationResponse.Valid)
            _notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));

        return validationResponse.Valid;
    }

    protected bool EntitiesValidation(List<T> entities)
    {
        foreach (var validationResponse in entities.Select(entity => _validate.Validation(entity)).Where(validationResponse => !validationResponse.Valid))
        {
            _notification.CreateNotifications(DomainNotification.CreateNotifications(validationResponse.Errors));
        }

        return !_notification.HasNotification();
    }
}

