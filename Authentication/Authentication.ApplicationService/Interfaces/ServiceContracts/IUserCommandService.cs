using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IUserCommandService : IDisposable
{
    Task<bool> RegisterAsync(UserRegisterRequest userRegisterRequest);
    Task<bool> ChangePasswordAsync(UserIdentityChangePasswordRequest userIdentityChangePasswordRequest);
}