using System.Linq.Expressions;
using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.NotificatioTrace;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.Domain.Extensions;
using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Handlers.ValidationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Microsoft.AspNetCore.Identity;

namespace Authentication.ApplicationService.Services.UserServices;

public class UserCommandService : BaseService<User>, IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public UserCommandService(IUserRepository userRepository,
        IValidate<User> validate,
        INotificationHandler notification,
        IUserMapper userMapper)
        : base(notification, validate)

    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public void Dispose() => _userRepository.Dispose();

    public async Task<bool> RegisterAsync(UserRegisterRequest accountRegisterRequest)
    {
        if (await _userRepository.HaveInTheDatabaseAsync(u =>
                u.NormalizedUserName == accountRegisterRequest.Login.ToUpper()))
        {
            return _notification.CreateNotification(UserIdentityServiceTrace.CreateIdentityAccountMethod,
                EMessage.Exist.GetDescription().FormatTo("Login"));
        }

        var accountIdentity = _userMapper.DtoRegisterToDomain(accountRegisterRequest);

        if (!await EntityValidationAsync(accountIdentity))
            return false;

        var saveResult = await _userRepository.SaveAsync(accountIdentity);

        if (!saveResult.Succeeded)
            AddIdentityErrors(saveResult);

        return saveResult.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(UserIdentityChangePasswordRequest accountIdentityChangePasswordRequest)
    {
        var userIdentity = await _userRepository.FindByPredicateWithSelectorAsync(
            u => u.Id == accountIdentityChangePasswordRequest.UserIdentityId,
            QueryProjectionForChangePassword());

        if (userIdentity is null)
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword,
                EMessage.NotFound.GetDescription().FormatTo("User"));

        if (!accountIdentityChangePasswordRequest.NewPassword.ValidatePassword())
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword,
                "A senha não atende aos requisitos.");

        var updateResult = await _userRepository.ChangePasswordAsync(userIdentity,
            accountIdentityChangePasswordRequest.OldPassword,
            accountIdentityChangePasswordRequest.NewPassword);

        if (!updateResult.Succeeded)
            AddIdentityErrors(updateResult);

        return updateResult.Succeeded;
    }

    private static Expression<Func<User, User>> QueryProjectionForChangePassword() =>
        u => new User
        {
            Id = u.Id,
            UserName = u.UserName,
            PasswordHash = u.PasswordHash,
            SecurityStamp = u.SecurityStamp,
            ConcurrencyStamp = u.ConcurrencyStamp
        };

    private void AddIdentityErrors(IdentityResult identityResult)
    {
        foreach (var error in identityResult.Errors)
        {
            _notification.CreateNotification(new DomainNotification(UserIdentityServiceTrace.IdentityValidationMethod,
                error.Description));
        }
    }
}