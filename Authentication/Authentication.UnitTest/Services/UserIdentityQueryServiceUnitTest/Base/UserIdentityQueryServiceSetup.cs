using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Services.UserIdentityServices;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Domain.Providers;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest.Base;
public abstract class UserIdentityQueryServiceSetup
{
    protected readonly Mock<IUserIdentityRepository> _userIdentityRepository;
    protected readonly Mock<IUserIdentityMapper> _userIdentityMapper;
    protected readonly UserIdentityQueryService _userIdentityQueryService;
    protected readonly JwtTokenOptions _jwtTokenOptions;
    protected readonly string _securityAlgorithms = SecurityAlgorithms.HmacSha256;

    public UserIdentityQueryServiceSetup()
    {
        _userIdentityRepository = new();
        _userIdentityMapper = new();
        _jwtTokenOptions = GetJwtTokenOptions();
        _userIdentityQueryService = new(_userIdentityRepository.Object,
                                        _userIdentityMapper.Object
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
