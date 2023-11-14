using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.UnitTest.NotificationsUnitTest.Base;

namespace Authentication.UnitTest.NotificationsUnitTest;
public sealed class HasNotificationMethodUnitTest : NotificationHandlerSetup
{
    [Fact]
    [Trait("Success", "Has notification is true")]
    public void HasNotification_ReturnTrue()
    {
        const string keyNotification = "Not Found";
        const string valueNotification = "Objeto não encontrado.";
        _notificationHandler.CreateNotification(new DomainNotification(keyNotification, valueNotification));

        var result = _notificationHandler.HasNotification();

        Assert.True(result);
    }

    [Fact]
    [Trait("Success", "Has notification is false")]
    public void HasNotification_ReturnFalse()
    {
        var result = _notificationHandler.HasNotification();

        Assert.False(result);
    }
}
