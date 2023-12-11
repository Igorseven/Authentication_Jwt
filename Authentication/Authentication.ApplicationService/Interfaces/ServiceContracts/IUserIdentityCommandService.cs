using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IUserIdentityCommandService : IDisposable
{
    Task<bool> CreateIdentityAccountAsync(
        UserIdentityRegisterRequest userIdentityRegisterRequest);
    Task<bool> ChangePasswordAsync(UserIdentityChangePasswordRequest userIdentityChangePasswordRequest);
}