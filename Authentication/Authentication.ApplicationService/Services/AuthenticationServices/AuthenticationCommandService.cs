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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.ApplicationService.Services.AuthenticationServices;
public sealed class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly INotificationHandler _notification;
    private readonly IUserIdentityQueryService _userIdentityQueryService;
    private readonly JwtTokenOptions _jwtTokenOptions;
    private readonly SymmetricSecurityKey _key;
    private const string _securityAlgorithm = SecurityAlgorithms.HmacSha256;

    public AuthenticationCommandService(IRefreshTokenRepository refreshTokenRepository,
                                        IUserIdentityQueryService userIdentityQueryService,
                                        INotificationHandler notification,
                                        IOptions<JwtTokenOptions> options)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userIdentityQueryService = userIdentityQueryService;
        _notification = notification;
        _jwtTokenOptions = options.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenOptions.JwtKey));
    }

    public void Dispose() => _refreshTokenRepository.Dispose();

    public async Task<AuthenticationLoginResponse> GenerateAccessTokenAsync(UserLogin userLogin)
    {
        if (!await _userIdentityQueryService.CheckLoginAndPasswordAsyncAsync(userLogin))
        {
            return new()
            {
                AccessToken = "",
                RefreshToken = "",
                Expiry = 0,
                Message = "Login ou senha inválido."
            };
        }

        await _refreshTokenRepository.DeleteAsync(userLogin.Login);

        var jwtToken = await GenerateJwtTokenAsync(userLogin.Login);
        var newRefreshToken = await GenerateRefreshTokenAsync(userLogin.Login!);

        return new()
        {
            AccessToken = jwtToken,
            RefreshToken = newRefreshToken,
            Expiry = _jwtTokenOptions.DurationInMinutes,
            Message = "autenticado com sucesso."
        };
    }

    public async Task<AuthenticationLoginResponse?> GenerateRefreshTokenAsync(UpdateAccessToken updateAccessToken)
    {
        var extractUserRequest = CreateExtractUserRequest(updateAccessToken.AccessToken);

        var userName = await _userIdentityQueryService.ExtractUserFromAccessTokenAsync(extractUserRequest);

        if (userName is null)
        {
            _notification.CreateNotification(AuthenticationServiceTrace.GenerateRefreshTokenMethod, "Erro ao extrair credenciais do token expirado.");
            return null;
        }

        if (!await _refreshTokenRepository.HaveInTheDatabaseAsync(r => r.UserName == userName && r.Token == updateAccessToken.RefreshToken))
        {
            _notification.CreateNotification(AuthenticationServiceTrace.GenerateRefreshTokenMethod, "Refresh Token inválido.");
            return null;
        }

        await _refreshTokenRepository.DeleteAsync(userName, updateAccessToken.RefreshToken);

        var jwtToken = await GenerateJwtTokenAsync(userName);
        var newRefreshToken = await GenerateRefreshTokenAsync(userName);

        return new AuthenticationLoginResponse
        {
            AccessToken = jwtToken,
            RefreshToken = newRefreshToken,
            Expiry = _jwtTokenOptions.DurationInMinutes,
            Message = "autenticado com sucesso."
        };
    }

    private async Task<string> GenerateJwtTokenAsync(string userName)
    {
        var claims = await _userIdentityQueryService.GetUseClaimsAsync(userName!);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtTokenOptions.DurationInMinutes),
            Issuer = _jwtTokenOptions.Issuer,
            Audience = _jwtTokenOptions.Audience,
            SigningCredentials = new SigningCredentials(_key, _securityAlgorithm)
        };

        var tokeHandler = new JwtSecurityTokenHandler();
        var token = tokeHandler.CreateToken(tokenDescription);

        return tokeHandler.WriteToken(token);
    }

    private async Task<string> GenerateRefreshTokenAsync(string userName)
    {
        using var randomGenerator = RandomNumberGenerator.Create();

        var randoNumbers = new byte[32];
        randomGenerator.GetBytes(randoNumbers);
        var createToken = Convert.ToBase64String(randoNumbers);

        var refreshToken = new RefreshToken
        {
            UserName = userName,
            Token = createToken
        };

        await _refreshTokenRepository.SaveAsync(refreshToken);

        return createToken;
    }

    private ExtractUserRequest CreateExtractUserRequest(string accessToken) =>
        new()
        {
            AccessToken = accessToken,
            SecurityAlgorithm = _securityAlgorithm,
            TokenValidationParameters = new()
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
