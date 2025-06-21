using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.AuthenticationResponse;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IAuthenticationCommandService : IDisposable
{
    Task<AuthenticationLoginResponse?> GenerateAccessTokenAsync(AuthenticationRequest authenticationRequest);
    Task<AuthenticationLoginResponse?> GenerateRefreshTokenAsync(UpdateAccessToken updateAccessToken);
}
