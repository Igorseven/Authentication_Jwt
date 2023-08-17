using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.UnitTest.NotificationsUnitTest.Base;

namespace Authentication.UnitTest.NotificationsUnitTest;
public sealed class CreateNotificationMethodsUnitTest : NotificationHandlerSetup
{
    [Fact]
    [Trait("Success", "Create notification and return false")]
    public void CreateNotification_HasNotificationAndReturnFalse()
    {
        const string keyNotification = "Not Found";
        const string valueNotification = "Objeto não encontrado.";

        var result = _notificationHandler.CreateNotification(keyNotification, valueNotification);

        Assert.True(_notificationHandler.HasNotification());
        Assert.False(result);
    }


    [Fact]
    [Trait("Success", "Create new notification")]
    public void CreateNotification_HasNotification()
    {
        const string keyNotification = "Not Found";
        const string valueNotification = "Objeto não encontrado.";

       _notificationHandler.CreateNotification(new DomainNotification(keyNotification, valueNotification));

        Assert.True(_notificationHandler.HasNotification());
    }

    [Fact]
    [Trait("Success", "Create notifications")]
    public void CreateNotifications_HasNotification()
    {
        const string keyNotification = "Not Found";
        const string valueNotification = "Objeto não encontrado.";

        var notifications = new List<DomainNotification>
        {
            { new DomainNotification(keyNotification, valueNotification) },
            { new DomainNotification(keyNotification, valueNotification) },
            { new DomainNotification(keyNotification, valueNotification) }
        };

        _notificationHandler.CreateNotifications(notifications);

        Assert.True(_notificationHandler.HasNotification());
    }
}
