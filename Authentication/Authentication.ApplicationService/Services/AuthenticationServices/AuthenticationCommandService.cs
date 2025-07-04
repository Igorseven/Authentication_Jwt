﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.AuthenticationResponse;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.NotificatioTrace;
using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Domain.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.ApplicationService.Services.AuthenticationServices;

public sealed class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly INotificationHandler _notification;
    private readonly IUserQueryService _userQueryService;
    private readonly JwtTokenOptions _jwtTokenOptions;
    private readonly SymmetricSecurityKey _key;
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    public AuthenticationCommandService(IUserTokenRepository userTokenRepository,
        IUserQueryService userQueryService,
        INotificationHandler notification,
        IOptions<JwtTokenOptions> options)
    {
        _userTokenRepository = userTokenRepository;
        _userQueryService = userQueryService;
        _notification = notification;
        _jwtTokenOptions = options.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.JwtKey));
    }

    public void Dispose() => _userTokenRepository.Dispose();

    public async Task<AuthenticationLoginResponse?> GenerateAccessTokenAsync(AuthenticationRequest userLogin)
    {
        var user = await _userQueryService.FindByUserNameAsync(userLogin.Login);
        
        if (user is null)
        {
            _notification.CreateNotification(
                AuthenticationServiceTrace.GenerateAccessTokenMethod,
                "Login ou senha inválido.");

            return null;
        }

        await _userTokenRepository.DeleteAsync(r => r.Name == userLogin.Login &&
                                                    r.LoginProvider == userLogin.SystemOrigin.ToString());

        var jwtToken = await GenerateJwtTokenAsync(userLogin.Login);
        var newRefreshToken = await GenerateRefreshTokenAsync(
            userLogin.Login,
            userLogin.SystemOrigin,
            user.Id);

        return new AuthenticationLoginResponse
        {
            AccessToken = jwtToken,
            RefreshToken = newRefreshToken,
            Expiry = _jwtTokenOptions.DurationInMinutes
        };
    }

    public async Task<AuthenticationLoginResponse?> GenerateRefreshTokenAsync(UpdateAccessToken updateAccessToken)
    {
        var extractUserRequest = CreateExtractUserRequest(updateAccessToken.AccessToken);

        var user = await _userQueryService.ExtractUserFromAccessTokenAsync(extractUserRequest);

        if (user is null)
        {
            _notification.CreateNotification(AuthenticationServiceTrace.GenerateRefreshTokenMethod,
                "Erro ao extrair credenciais do token expirado.");
            return null;
        }

        if (!await _userTokenRepository.HaveInTheDatabaseAsync(r =>
                r.Name == user.Value.userName && r.Value == updateAccessToken.RefreshToken))
        {
            _notification.CreateNotification(AuthenticationServiceTrace.GenerateRefreshTokenMethod,
                "Refresh Token inválido.");
            return null;
        }

        await _userTokenRepository.DeleteAsync(
            r => r.Name == user.Value.userName &&
                 r.Value == updateAccessToken.RefreshToken);

        var jwtToken = await GenerateJwtTokenAsync(user.Value.userName);
        var newRefreshToken = await GenerateRefreshTokenAsync(          
            user.Value.userName,
            updateAccessToken.SystemOrigin,
            user.Value.userId);

        return new AuthenticationLoginResponse
        {
            AccessToken = jwtToken,
            RefreshToken = newRefreshToken,
            Expiry = _jwtTokenOptions.DurationInMinutes
        };
    }

    private async Task<string> GenerateJwtTokenAsync(string userName)
    {
        var claims = await _userQueryService.GetUseClaimsAsync(userName);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtTokenOptions.DurationInMinutes),
            Issuer = _jwtTokenOptions.Issuer,
            Audience = _jwtTokenOptions.Audience,
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithm)
        };

        var tokeHandler = new JwtSecurityTokenHandler();
        var token = tokeHandler.CreateToken(tokenDescription);

        return tokeHandler.WriteToken(token);
    }

    private async Task<string> GenerateRefreshTokenAsync(string userName, Guid systemOrigin, Guid userId)
    {
        using var randomGenerator = RandomNumberGenerator.Create();

        var randoNumbers = new byte[32];
        randomGenerator.GetBytes(randoNumbers);
        var createToken = Convert.ToBase64String(randoNumbers);

        var refreshToken = new UserToken
        {
            UserId = userId,
            Name = userName,
            LoginProvider = systemOrigin.ToString(),
            Value = createToken
        };

        await _userTokenRepository.SaveAsync(refreshToken);

        return createToken;
    }

    private ExtractUserRequest CreateExtractUserRequest(string accessToken) =>
        new()
        {
            AccessToken = accessToken,
            SecurityAlgorithm = SecurityAlgorithm,
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = _jwtTokenOptions.Audience,
                ValidIssuer = _jwtTokenOptions.Issuer,
                IssuerSigningKey = _key,
            }
        };
}