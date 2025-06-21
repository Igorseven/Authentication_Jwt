using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Services.UserIdentityServices;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Domain.Providers;
using Moq;

namespace Authentication.UnitTest.Services.UserQueryServiceUnitTest.Base;
public abstract class UserIdentityQueryServiceSetup
{
    protected readonly Mock<IUserIdentityRepository> UserIdentityRepository;
    protected readonly Mock<IUserIdentityMapper> UserIdentityMapper;
    protected readonly UserQueryService UserQueryService;
    protected readonly JwtTokenOptions JwtTokenOptions;
    protected readonly string SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256;

    protected UserIdentityQueryServiceSetup()
    {
        UserIdentityRepository = new Mock<IUserIdentityRepository>();
        UserIdentityMapper = new Mock<IUserIdentityMapper>();
        JwtTokenOptions = GetJwtTokenOptions();
        UserQueryService = new UserQueryService(UserIdentityRepository.Object,
                                        UserIdentityMapper.Object
                                        );
    }

    private static JwtTokenOptions GetJwtTokenOptions() =>
       new()
       {
           Audience = "AsecureRestApiUser",
           Issuer = "AuthenticationApi",
           DurationInMinutes = 60,
           JwtKey = "_habKLEnMAUeb-ZXAiLllIiAr.dev"
       };
}
