using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Mappers.UserIdentityMapperUnitTest.Base;

namespace Authentication.UnitTest.Mappers.UserIdentityMapperUnitTest;
public sealed class DtoUserIdentityRegisterRequestToDomainMethodUnitTest : UserIdentityMapperSetup
{
    public static IEnumerable<object[]> UserIdentityRegisterRequestPerfectSetting()
    {
        yield return new object[]
        {
            new UserIdentityRegisterRequest
            {
                Login = "userlogin@test.com",
                RegistrationDate = DateTime.Now,
                UserPassword = new UserPassword
                {
                    Password = "@Password2023",
                    PasswordConfirm = "@Password2023"
                }
            }
        };
    }

    [Theory]
    [Trait("Mapping", "Dto user identity register to domain")]
    [MemberData(nameof(UserIdentityRegisterRequestPerfectSetting))]
    public void DtoUserIdentityRegisterRequestToDomain_ReturnAccountIdentity(UserIdentityRegisterRequest accountIdentityRegisterRequest)
    {
        var mappingResult = _accountIdentityMapper.DtoUserIdentityRegisterRequestToDomain(accountIdentityRegisterRequest);

        Assert.NotNull(mappingResult);
        Assert.Equal(accountIdentityRegisterRequest.Login, mappingResult.UserName);
        Assert.Equal(accountIdentityRegisterRequest.UserPassword.PasswordConfirm, mappingResult.PasswordHash);
        Assert.Null(mappingResult.Email);
        Assert.Null(mappingResult.PhoneNumber);
        Assert.Equal(EUserType.Client, mappingResult.UserType);
        Assert.Equal(EUserStatus.Active, mappingResult.UserStatus);
    }
}
