using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Extensions;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Authentication.ApplicationService.Services.UserIdentityServices;
public sealed class UserIdentityQueryService : IUserIdentityQueryService
{
    private readonly IUserIdentityRepository _accountIdentityRepository;
    private readonly IUserIdentityMapper _accountIdentityMapper;

    public UserIdentityQueryService(IUserIdentityRepository accountIdentityRepository,
                                       IUserIdentityMapper accountIdentityMapper)
    {
        _accountIdentityRepository = accountIdentityRepository;
        _accountIdentityMapper = accountIdentityMapper;
    }

    public async Task<bool> CheckLoginAndPasswordAsyncAsync(UserLogin login)
    {
        var loginResult = await _accountIdentityRepository.PasswordSignInAsync(login.Login, login.Password);

        return loginResult.Succeeded;
    }

    public async Task<UserIdentityDataResponse?> FindUserIdentityDataAsync(string userName)
    {
        var userIdentity = await _accountIdentityRepository.FindByPredicateWithSelectorAsync(a => a.NormalizedUserName == userName.ToUpper(),
                                                                                             QueryProjectionUserIdentityData(),
                                                                                             true);

        if (userIdentity is null) return null;

        return _accountIdentityMapper.DomainToDtoUserIdentityData(userIdentity);
    }

    public async Task<UserIdentity?> FindByUserNameAsync(string userName) =>
        await _accountIdentityRepository.FindByPredicateWithSelectorAsync(a => a.NormalizedUserName == userName.ToUpper(), null, true);

    public async Task<string?> ExtractUserFromAccessTokenAsync(ExtractUserRequest extractUserRequest)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        ClaimsPrincipal principal = tokenHandler.ValidateToken(extractUserRequest.AccessToken, extractUserRequest.TokenValidationParameters, out SecurityToken securityToken);

        if (principal?.Identity?.Name is null ||
            !await _accountIdentityRepository.HaveInTheDatabaseAsync(a => a.UserName == principal.Identity.Name))
        {
            return null;
        }

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(extractUserRequest.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal.Identity.Name;
    }


    public async Task<List<Claim>> GetUseClaimsAsync(string userName)
    {
        var accountIdentity = await _accountIdentityRepository.FindByPredicateWithSelectorAsync(a => a.NormalizedUserName == userName.ToUpper(), null, true);
        var userRoles = await _accountIdentityRepository.FindAllRolesAsync(accountIdentity!);

        var claims = DefaultClaims(accountIdentity!);

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }


    private static List<Claim> DefaultClaims(UserIdentity userIdentity) =>
        new()
        {
            new Claim(ClaimTypes.Name, userIdentity.UserName!),
            new Claim(ClaimTypes.Role, userIdentity.UserType.GetDescription())
        };


    private static Expression<Func<UserIdentity, UserIdentity>> QueryProjectionUserIdentityData() =>
        a => new UserIdentity
        {
            Id = a.Id,
            NormalizedUserName = a.NormalizedUserName,
            UserStatus = a.UserStatus,
            UserType = a.UserType
        };
}

