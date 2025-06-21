using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Services.UserIdentityServices;
using Authentication.Domain.Entities;
using Authentication.Domain.Handlers.ValidationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Moq;

namespace Authentication.UnitTest.Services.UserCommandServiceUnitTest.Base;

public abstract class UserIdentityCommandServiceSetup
{
    protected readonly Mock<IUserIdentityRepository> UserIdentityRepository;
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<User>> Validate;
    protected readonly Mock<IUserIdentityMapper> UserIdentityMapper;
    protected readonly UserCommandService UserCommandService;
    protected readonly ValidationResponse ValidationResponse;
    private readonly Dictionary<string, string> _errors;

    protected UserIdentityCommandServiceSetup()
    {
        UserIdentityRepository = new Mock<IUserIdentityRepository>();
        NotificationHandler = new Mock<INotificationHandler>();
        Validate = new Mock<IValidate<User>>();
        UserIdentityMapper = new Mock<IUserIdentityMapper>();
        _errors = new Dictionary<string, string>();
        ValidationResponse = ValidationResponse.CreateResponse(_errors);
        UserCommandService = new UserCommandService(
            UserIdentityRepository.Object,
            Validate.Object,
            NotificationHandler.Object,
            UserIdentityMapper.Object
        );
    }

    protected void CreateInvalidDataNotification() =>
        _errors.Add("Error", "Error");
}