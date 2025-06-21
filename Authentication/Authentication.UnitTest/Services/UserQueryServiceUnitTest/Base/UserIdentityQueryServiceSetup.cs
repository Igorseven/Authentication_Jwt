using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Services.UserServices;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Domain.Providers;
using Moq;

namespace Authentication.UnitTest.Services.UserQueryServiceUnitTest.Base;
public abstract class UserIdentityQueryServiceSetup
{
    protected readonly Mock<IUserRepository> UserIdentityRepository;
    protected readonly Mock<IUserMapper> UserIdentityMapper;
    protected readonly UserQueryService UserQueryService;
    protected readonly JwtTokenOptions JwtTokenOptions;
    protected readonly string SecurityAlgorithms = Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256;

    protected UserIdentityQueryServiceSetup()
    {
        UserIdentityRepository = new Mock<IUserRepository>();
        UserIdentityMapper = new Mock<IUserMapper>();
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
           JwtKey = "_habKLEnMAUeb-ZXAiLllIiAr.dev",
           RequireHttpsMetadata = true
       };
}
