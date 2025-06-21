using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;
using System.Security.Claims;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IUserQueryService
{
    Task<UserSimpleResponse?> FindByLoginAsync(string userName);
    Task<bool> CheckLoginAndPasswordAsync(AuthenticationRequest login);
    Task<List<Claim>> GetUseClaimsAsync(string userName);
    Task<User?> FindByUserNameAsync(string userName);
    Task<(string userName, Guid userId)?> ExtractUserFromAccessTokenAsync(ExtractUserRequest extractUserRequest);
}
