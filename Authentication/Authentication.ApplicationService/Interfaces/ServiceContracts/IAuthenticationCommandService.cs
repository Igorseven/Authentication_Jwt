using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.AuthenticationResponse;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IAuthenticationCommandService : IDisposable
{
    Task<AuthenticationLoginResponse?> GenerateAccessTokenAsync(UserLogin userLogin);
    Task<AuthenticationLoginResponse?> GenerateRefreshTokenAsync(UpdateAccessToken updateAccessToken);
}
