using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
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

namespace Authentication.ApplicationService.Services.UserIdentityServices;
public class UserIdentityCommandService : BaseService<UserIdentity>, IUserIdentityCommandService
{
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly IUserIdentityMapper _userIdentityMapper;

    public UserIdentityCommandService(IUserIdentityRepository userIdentityRepository,
                                      IValidate<UserIdentity> validate,
                                      INotificationHandler notification,
                                      IUserIdentityMapper userIdentityMapper)
        : base(notification, validate)

    {
        _userIdentityRepository = userIdentityRepository;
        _userIdentityMapper = userIdentityMapper;
    }

    public void Dispose() => _userIdentityRepository.Dispose();

    public async Task<bool> CreateIdentityAccountAsync(UserIdentityRegisterRequest accountIdentityRegisterRequest)
    {
        if (await _userIdentityRepository.HaveInTheDatabaseAsync(u => u.NormalizedUserName == accountIdentityRegisterRequest.Login.ToUpper()))
            return _notification.CreateNotification(UserIdentityServiceTrace.CreateIdentityAccountMethod, EMessage.Exist.GetDescription().FormatTo("Login"));

        var accountIdentity = _userIdentityMapper.DtoUserIdentityRegisterRequestToDomain(accountIdentityRegisterRequest);

        if (!await EntityValidationAsync(accountIdentity)) return false;

        var saveResult = await _userIdentityRepository.SaveAsync(accountIdentity);

        if (!saveResult.Succeeded)
            AddIdentityErrors(saveResult);

        return saveResult.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(UserIdentityChangePasswordRequest accountIdentityChangePasswordRequest)
    {
        var userIdentity = await _userIdentityRepository.FindByPredicateWithSelectorAsync(u => u.Id == accountIdentityChangePasswordRequest.UserIdentityId,
                                                                                          QueryProjectionForChangePassword());

        if (userIdentity is null)
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword, EMessage.NotFound.GetDescription().FormatTo("User"));

        if (!accountIdentityChangePasswordRequest.NewPassword.ValidatePassword())
            return _notification.CreateNotification(UserIdentityServiceTrace.ChangePassword, "A senha não atende aos requisitos.");

        var updateResult = await _userIdentityRepository.ChangePasswordAsync(userIdentity,
                                                                             accountIdentityChangePasswordRequest.OldPassword,
                                                                             accountIdentityChangePasswordRequest.NewPassword);

        if (!updateResult.Succeeded)
            AddIdentityErrors(updateResult);

        return updateResult.Succeeded;
    }

    private static Expression<Func<UserIdentity, UserIdentity>> QueryProjectionForChangePassword() =>
         u => new UserIdentity
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
            _notification.CreateNotification(new DomainNotification(UserIdentityServiceTrace.IdentityValidationMethod, error.Description));
        }
    }
}
