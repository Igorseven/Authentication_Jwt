using Authentication.Domain.Handlers.NotificationHandler;

namespace Authentication.UnitTest.NotificationsUnitTest.Base;
public abstract class NotificationHandlerSetup
{
    protected readonly NotificationHandler _notificationHandler = new();
    
}
