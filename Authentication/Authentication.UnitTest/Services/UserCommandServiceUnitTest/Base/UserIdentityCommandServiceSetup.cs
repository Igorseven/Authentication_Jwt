using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Services.UserServices;
using Authentication.Domain.Entities;
using Authentication.Domain.Handlers.ValidationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Moq;

namespace Authentication.UnitTest.Services.UserCommandServiceUnitTest.Base;

public abstract class UserIdentityCommandServiceSetup
{
    protected readonly Mock<IUserRepository> UserIdentityRepository;
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<User>> Validate;
    protected readonly Mock<IUserMapper> UserIdentityMapper;
    protected readonly UserCommandService UserCommandService;
    protected readonly ValidationResponse ValidationResponse;
    private readonly Dictionary<string, string> _errors;

    protected UserIdentityCommandServiceSetup()
    {
        UserIdentityRepository = new Mock<IUserRepository>();
        NotificationHandler = new Mock<INotificationHandler>();
        Validate = new Mock<IValidate<User>>();
        UserIdentityMapper = new Mock<IUserMapper>();
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