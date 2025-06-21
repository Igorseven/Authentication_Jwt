using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.Services.AuthenticationServices;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Domain.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest.Base;

public abstract class AuthenticationCommandServiceSetup
{
    protected readonly Mock<IUserTokenRepository> RefreshTokenRepository;
    protected readonly Mock<INotificationHandler> Notification;
    protected readonly Mock<IUserQueryService> UserQueryService;

    protected readonly SymmetricSecurityKey Key;
    protected readonly AuthenticationCommandService AuthenticationCommandService;
    protected readonly JwtTokenOptions JwtTokenOptions;
    protected const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    protected AuthenticationCommandServiceSetup()
    {
        var options = Options.Create(GetJwtTokenOptions());
        RefreshTokenRepository = new Mock<IUserTokenRepository>();
        Notification = new Mock<INotificationHandler>();
        UserQueryService = new Mock<IUserQueryService>();
        JwtTokenOptions = GetJwtTokenOptions();
        Key = new SymmetricSecurityKey("_habKLEnMAUeb-ZXAiLllIiAr.dev"u8.ToArray());
        AuthenticationCommandService = new AuthenticationCommandService(
            RefreshTokenRepository.Object,
            UserQueryService.Object,
            Notification.Object,
            options);
    }


    private static JwtTokenOptions GetJwtTokenOptions() =>
        new()
        {
            Audience = "Audience",
            Issuer = "Issuer",
            DurationInMinutes = 60,
            JwtKey = "_habKLEnMAUeb-ZXAiLllIiAr.dev",
            RequireHttpsMetadata = true
        };
}