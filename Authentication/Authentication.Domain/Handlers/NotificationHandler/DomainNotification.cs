namespace Authentication.Domain.Handlers.NotificationHandler;
public sealed class DomainNotification
{
    public string Key { get; set; }
    public string Value { get; set; }

    public DomainNotification(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public static IEnumerable<DomainNotification> CreateNotifications(Dictionary<string, string> notifications)
    {
        return notifications.Select(notification => new DomainNotification(notification.Key, notification.Value));
    }
}
