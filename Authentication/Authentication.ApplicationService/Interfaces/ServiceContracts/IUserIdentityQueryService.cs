using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;
using System.Security.Claims;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IUserIdentityQueryService
{
    Task<UserIdentityDataResponse?> FindUserIdentityDataAsync(string userName);
    Task<bool> CheckLoginAndPasswordAsyncAsync(UserLogin login);
    Task<List<Claim>> GetUseClaimsAsync(string userName);
    Task<UserIdentity?> FindByUserNameAsync(string userName);
    Task<string?> ExtractUserFromAccessTokenAsync(ExtractUserRequest extractUserRequest);
}
