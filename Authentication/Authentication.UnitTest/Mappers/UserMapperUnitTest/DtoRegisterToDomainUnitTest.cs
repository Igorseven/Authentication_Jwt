using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Mappers.UserMapperUnitTest.Base;

namespace Authentication.UnitTest.Mappers.UserMapperUnitTest;
public sealed class DtoRegisterToDomainUnitTest : UserMapperSetup
{
    public static IEnumerable<object[]> UserRegisterRequestPerfectSetting()
    {
        yield return new object[]
        {
            new UserRegisterRequest
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
    [Trait("Mapping", "Dto user register to domain")]
    [MemberData(nameof(UserRegisterRequestPerfectSetting))]
    public void DtoRegisterRequestToDomain_ReturnUser(UserRegisterRequest dtoRegister)
    {
        var mappingResult = AccountMapper.DtoRegisterToDomain(dtoRegister);

        Assert.NotNull(mappingResult);
        Assert.Equal(dtoRegister.Login, mappingResult.UserName);
        Assert.Equal(dtoRegister.UserPassword.PasswordConfirm, mappingResult.PasswordHash);
        Assert.Null(mappingResult.Email);
        Assert.Null(mappingResult.PhoneNumber);
        Assert.Equal(EUserType.Client, mappingResult.Type);
        Assert.Equal(EUserStatus.Active, mappingResult.Status);
    }
}
