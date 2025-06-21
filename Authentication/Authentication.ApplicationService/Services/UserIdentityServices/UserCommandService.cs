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
using System.Linq.Expressions;
using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;

namespace Authentication.ApplicationService.Services.UserIdentityServices;

public class UserCommandService : BaseService<User>, IUserCommandService
{
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly IUserIdentityMapper _userIdentityMapper;

    public UserCommandService(IUserIdentityRepository userIdentityRepository,
        IValidate<User> validate,
        INotificationHandler notification,
        IUserIdentityMapper userIdentityMapper)
        : base(notification, validate)

    {
        _userIdentityRepository = userIdentityRepository;
        _userIdentityMapper = userIdentityMapper;
    }

    public void Dispose() => _userIdentityRepository.Dispose();

    public async Task<bool> RegisterAsync(UserRegisterRequest accountRegisterRequest)
    {
        if (await _userIdentityRepository.HaveInTheDatabaseAsync(u =>
                u.NormalizedUserName == accountRegisterRequest.Login.ToUpper()))
        {
            return _notification.CreateNotification(UserIdentityServiceTrace.CreateIdentityAccountMethod,
                EMessage.Exist.GetDescription().FormatTo("Login"));
        }

        var accountIdentity = _userIdentityMapper.DtoRegisterToDomain(accountRegisterRequest);

        if (!await EntityValidationAsync(accountIdentity))
            return false;

        var saveResult = await _userIdentityRepository.SaveAsync(accountIdentity);

        if (!saveResult.Succeeded)
            AddIdentityErrors(saveResult);

        return saveResult.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(UserIdentityChangePasswordRequest accountIdentityChangePasswordRequest)
    {
        var userIdentity = await _userIdentityRepository.FindByPredicateWithSelectorAsync(
            u => u.Id == accountIdentityChangePasswordRequest.UserIdentityId,
            QueryProjectionForChangePassword());

        if (userIdentity is null)
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword,
                EMessage.NotFound.GetDescription().FormatTo("User"));

        if (!accountIdentityChangePasswordRequest.NewPassword.ValidatePassword())
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword,
                "A senha não atende aos requisitos.");

        var updateResult = await _userIdentityRepository.ChangePasswordAsync(userIdentity,
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