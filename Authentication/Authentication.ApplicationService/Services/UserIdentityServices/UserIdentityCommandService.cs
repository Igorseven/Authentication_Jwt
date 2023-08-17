using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.NotificatioTrace;
using Authentication.ApplicationService.Services.Base;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.Domain.Extensions;
using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Authentication.ApplicationService.Services.UserIdentityServices;
public class UserIdentityCommandService : BaseService<UserIdentity>, IUserIdentityCommandService
{
    private readonly IUserIdentityRepository _accountIdentityRepository;
    private readonly IUserIdentityMapper _accountIdentityMapper;

    public UserIdentityCommandService(IUserIdentityRepository userIdentityRepository,
                                         IValidate<UserIdentity> validate,
                                         INotificationHandler notification,
                                         IUserIdentityMapper userIdentityMapper)
        : base(notification, validate)

    {
        _accountIdentityRepository = userIdentityRepository;
        _accountIdentityMapper = userIdentityMapper;
    }

    public void Dispose() => _accountIdentityRepository.Dispose();

    public async Task<bool> CreateIdentityAccountAsync(UserIdentityRegisterRequest accountIdentityRegisterRequest)
    {
        if (await _accountIdentityRepository.HaveInTheDatabaseAsync(i => i.NormalizedUserName == accountIdentityRegisterRequest.Login!.ToUpper()))
            return _notification.CreateNotification(UserIdentityServiceTrace.CreateIdentityAccountMethod, EMessage.Exist.GetDescription().FormatTo("Login"));

        var accountIdentity = _accountIdentityMapper.DtoUserIdentityRegisterRequestToDomain(accountIdentityRegisterRequest);

        if (!await EntityValidationAsync(accountIdentity)) return false;

        var saveResult = await _accountIdentityRepository.SaveAsync(accountIdentity);

        if (!saveResult.Succeeded)
            AddIdentityErrors(saveResult);

        return saveResult.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(UserIdentityChangePasswordRequest accountIdentityChangePasswordRequest)
    {
        var accountIdentity = await _accountIdentityRepository.FindByPredicateWithSelectorAsync(a => a.Id == accountIdentityChangePasswordRequest.UserIdentityId,
                                                                                                QueryProjectionForChangePassword(),
                                                                                                false);

        if (accountIdentity is null)
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword, EMessage.NotFound.GetDescription().FormatTo("User"));

        var isValid = await _accountIdentityRepository.PasswordSignInAsync(accountIdentity.UserName!, accountIdentityChangePasswordRequest.OldPassword);

        if (!isValid.Succeeded)
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword, "Senha inválida.");

        accountIdentity.PasswordHash = accountIdentityChangePasswordRequest.NewPassword;

        if (!await EntityValidationAsync(accountIdentity)) return false;

        var updateResult = await _accountIdentityRepository.ResetPasswordAsync(accountIdentity, accountIdentityChangePasswordRequest.NewPassword);

        if (!updateResult.Succeeded)
            AddIdentityErrors(updateResult);

        return updateResult.Succeeded;
    }

    private static Expression<Func<UserIdentity, UserIdentity>>? QueryProjectionForChangePassword() =>
        a => new UserIdentity
        {
            Id = a.Id,
            UserName = a.UserName,
            PasswordHash = a.PasswordHash,
            UserStatus = a.UserStatus
        };

    private void AddIdentityErrors(IdentityResult identityResult)
    {
        foreach (var error in identityResult.Errors)
        {
            _notification.CreateNotification(new DomainNotification(UserIdentityServiceTrace.IdentityValidationMethod, error.Description));
        }
    }
}
