using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Services.UserIdentityServices;
using Authentication.Domain.Entities;
using Authentication.Domain.Handlers.ValidationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Moq;


namespace Authentication.UnitTest.Services.UserIdentityCommandServiceUnitTest.Base;
public abstract class UserIdentityCommandServiceSetup
{
    protected readonly Mock<IUserIdentityRepository> _userIdentityRepository;
    protected readonly Mock<INotificationHandler> _notificationHandler;
    protected readonly Mock<IValidate<UserIdentity>> _validate;
    protected readonly Mock<IUserIdentityMapper> _userIdentityMapper;
    protected readonly UserIdentityCommandService _userIdentityCommandService;
    protected readonly ValidationResponse _validationResponse;
    private readonly Dictionary<string, string> _errors;

    public UserIdentityCommandServiceSetup()
    {
        _userIdentityRepository = new();
        _notificationHandler = new();
        _validate = new();
        _userIdentityMapper = new();
        _errors = new();
        _validationResponse = ValidationResponse.CreateResponse(_errors);
        _userIdentityCommandService = new UserIdentityCommandService(_userIdentityRepository.Object,
                                                                     _validate.Object,
                                                                     _notificationHandler.Object,
                                                                     _userIdentityMapper.Object
                                                                     );
    }

    protected void CreateInvalidDataNotification() =>
        _errors.Add("Error", "Error");
}
