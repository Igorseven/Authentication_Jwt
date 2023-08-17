using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.Services.AuthenticationServices;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Domain.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;

namespace Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest.Base;
public abstract class AuthenticationCommandServiceSetup
{
    protected readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    protected readonly Mock<INotificationHandler> _notification;
    protected readonly Mock<IUserIdentityQueryService> _userIdentityQueryService;
    protected readonly SymmetricSecurityKey _key;
    protected readonly AuthenticationCommandService _authenticationCommandService;
    protected readonly JwtTokenOptions _jwtTokenOptions;
    protected readonly IOptions<JwtTokenOptions> _options;
    protected const string _securityAlgorithm = SecurityAlgorithms.HmacSha256;

    public AuthenticationCommandServiceSetup()
    {
        _refreshTokenRepository = new();
        _notification = new();
        _userIdentityQueryService = new();
        _options = Options.Create(GetJwtTokenOptions());
        _jwtTokenOptions = GetJwtTokenOptions();
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("_habKLEnMAUeb-ZXAiLllIiAr.dev"));
        _authenticationCommandService = new AuthenticationCommandService(_refreshTokenRepository.Object,
                                                                         _userIdentityQueryService.Object,
                                                                         _notification.Object,
                                                                         _options);
    }


    private static JwtTokenOptions GetJwtTokenOptions() =>
        new()
        {
            Audience = "Audience",
            Issuer = "Issuer",
            DurationInMinutes = 60,
            JwtKey = "_habKLEnMAUeb-ZXAiLllIiAr.dev"
        };

}
