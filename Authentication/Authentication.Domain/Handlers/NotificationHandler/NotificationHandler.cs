using Authentication.Domain.Interfaces.OthersContracts;

namespace Authentication.Domain.Handlers.NotificationHandler;
public sealed class NotificationHandler : INotificationHandler
{
    private readonly List<DomainNotification> _notifications;

    public NotificationHandler()
    {
        _notifications = new List<DomainNotification>();
    }

    public List<DomainNotification> GetNotifications() => _notifications;

    public bool HasNotification() => _notifications.Any();

    public void CreateNotifications(IEnumerable<DomainNotification> notifications) => _notifications.AddRange(notifications);

    public void CreateNotification(DomainNotification notification) => _notifications.Add(notification);

    public bool CreateNotification(string key, string value)
    {
        _notifications.Add(new DomainNotification(key, value));

        return false;
    }
}
