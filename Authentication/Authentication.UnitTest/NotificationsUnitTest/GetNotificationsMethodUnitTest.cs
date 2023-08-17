using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.UnitTest.NotificationsUnitTest.Base;

namespace RegistrationManagement.UnitTest.Handlers.NotificationsUnitTest;
public sealed class GetNotificationsMethodUnitTest : NotificationHandlerSetup
{

    [Fact]
    [Trait("Success", "Get notifications")]
    public void GetNotifications_ReturnNotifications()
    {
        const string keyNotification = "Not Found";
        const string valueNotification = "Objeto não encontrado.";
        _notificationHandler.CreateNotification(new DomainNotification(keyNotification, valueNotification));
        
        var result = _notificationHandler.GetNotifications();

        Assert.NotNull(result);
    }

    [Fact]
    [Trait("Success", "Empty list")]
    public void GetNotifications_ReturnEmptList()
    {
        var result = _notificationHandler.GetNotifications();

        Assert.Empty(result);
    }


}
