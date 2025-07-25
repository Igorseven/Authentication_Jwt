﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Extensions;
using Authentication.Domain.Interfaces.RepositoryContracts;

namespace Authentication.ApplicationService.Services.UserServices;

public sealed class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public UserQueryService(IUserRepository userRepository,
        IUserMapper userMapper)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public async Task<bool> CheckLoginAndPasswordAsync(AuthenticationRequest login)
    {
        var loginResult = await _userRepository.PasswordSignInAsync(login.Login, login.Password);

        return loginResult.Succeeded;
    }

    public async Task<UserSimpleResponse?> FindByLoginAsync(string userName)
    {
        var userIdentity = await _userRepository.FindByPredicateWithSelectorAsync(
            u => u.NormalizedUserName == userName.ToUpper(),
            QueryProjectionUserIdentityData(),
            true);

        return userIdentity is null 
            ? null 
            : _userMapper.DomainToSimpleResponse(userIdentity);
    }

    public async Task<User?> FindByUserNameAsync(string userName) =>
        await _userRepository.FindByPredicateWithSelectorAsync(
            u => u.NormalizedUserName == userName.ToUpper(),
            null, 
            true);

    public async Task<(string userName, Guid userId)?> ExtractUserFromAccessTokenAsync(ExtractUserRequest extractUserRequest)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        var principal = tokenHandler.ValidateToken(
            extractUserRequest.AccessToken,
            extractUserRequest.TokenValidationParameters,
            out var securityToken);

        if (principal?.Identity?.Name is null ||
            !await _userRepository.HaveInTheDatabaseAsync(u => u.UserName == principal.Identity.Name))
        {
            return null;
        }

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(extractUserRequest.SecurityAlgorithm,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return (principal.Identity.Name, GetUserIdFromToken(extractUserRequest.AccessToken));
    }


    public async Task<List<Claim>> GetUseClaimsAsync(string userName)
    {
        var accountIdentity = await _userRepository.FindByPredicateWithSelectorAsync(
            u => u.NormalizedUserName == userName.ToUpper(),
            null, true);

        var userRoles = await _userRepository.FindAllRolesAsync(accountIdentity!);

        var claims = DefaultClaims(accountIdentity!);

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }


    private static List<Claim> DefaultClaims(User user) =>
        new()
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Type.GetDescription())
        };


    private static Expression<Func<User, User>> QueryProjectionUserIdentityData() =>
        u => new User
        {
            Id = u.Id,
            NormalizedUserName = u.NormalizedUserName,
            Status = u.Status,
            Type = u.Type
        };
    
    private static Guid GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var jwtToken = handler.ReadJwtToken(token);
        
        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "NameIdentifier"); 

        return Guid.Parse(userIdClaim!.Value);
    }
}