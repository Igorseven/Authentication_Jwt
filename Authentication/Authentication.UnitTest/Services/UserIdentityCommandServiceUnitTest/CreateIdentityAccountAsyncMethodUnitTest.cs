using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.Domain.Entities;
using Authentication.UnitTest.Services.UserIdentityCommandServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace Authentication.UnitTest.Services.AccountIdentityCommandServiceUnitTest;
public sealed class CreateIdentityAccountAsyncMethodUnitTest : UserIdentityCommandServiceSetup
{
    public static IEnumerable<object[]> UserIdentityRegisterRequestPerfectSetting()
    {
        yield return new object[]
        {
            new UserIdentityRegisterRequest
            {
                Login = "user@gmail.com",
                UserPassword = new UserPassword
                {
                    Password = "@Test2023",
                    PasswordConfirm = "@Test2023"
                },
                RegistrationDate = DateTime.Now
            }
        };
    }

    [Theory]
    [Trait("Success", "Perfect setting")]
    [MemberData(nameof(UserIdentityRegisterRequestPerfectSetting))]
    public async Task CreateUserIdentityAsync_PerfectSetting_ReturnTrue(UserIdentityRegisterRequest accountIdentityRegisterRequest)
    {
        _userIdentityRepository.Setup(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserIdentity>())).ReturnsAsync(false);
        _userIdentityMapper.Setup(m => m.DtoUserIdentityRegisterRequestToDomain(It.IsAny<UserIdentityRegisterRequest>())).Returns(It.IsAny<UserIdentity>());
        _validate.Setup(v => v.ValidationAsync(It.IsAny<UserIdentity>())).ReturnsAsync(_validationResponse);
        _userIdentityRepository.Setup(a => a.SaveAsync(It.IsAny<UserIdentity>())).ReturnsAsync(IdentityResult.Success);

        var serviceResult = await _userIdentityCommandService.CreateIdentityAccountAsync(accountIdentityRegisterRequest);

        Assert.True(serviceResult);
        _userIdentityRepository.Verify(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserIdentity>()), Times.Once());
        _userIdentityMapper.Verify(m => m.DtoUserIdentityRegisterRequestToDomain(It.IsAny<UserIdentityRegisterRequest>()), Times.Once());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Once());
        _userIdentityRepository.Verify(a => a.SaveAsync(It.IsAny<UserIdentity>()), Times.Once());
    }


    public static IEnumerable<object[]> UserIdentityForRegisterTheLoginIsAlreadyInTheDatabase()
    {
        yield return new object[]
        {
            new UserIdentityRegisterRequest
            {
                Login = "user@gmail.com",
                UserPassword = new UserPassword
                {
                    Password = "@Test2023",
                    PasswordConfirm = "@Test2023"
                },
                RegistrationDate = DateTime.Now
            }
        };
    }
    
    [Theory]
    [Trait("Failed", "The login is already in the database")]
    [MemberData(nameof(UserIdentityForRegisterTheLoginIsAlreadyInTheDatabase))]
    public async Task CreateUserIdentityAsync_PerfectSetting_ReturnFalse(UserIdentityRegisterRequest accountIdentityRegisterRequest)
    {
        _userIdentityRepository.Setup(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserIdentity>())).ReturnsAsync(true);

        var serviceResult = await _userIdentityCommandService.CreateIdentityAccountAsync(accountIdentityRegisterRequest);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserIdentity>()), Times.Once());
        _userIdentityMapper.Verify(m => m.DtoUserIdentityRegisterRequestToDomain(It.IsAny<UserIdentityRegisterRequest>()), Times.Never());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Never());
        _userIdentityRepository.Verify(a => a.SaveAsync(It.IsAny<UserIdentity>()), Times.Never());
    }


    public static IEnumerable<object[]> UserIdentityForRegisterAnErrorOccuredWhileTryingToRegisterIdentityData()
    {
        yield return new object[]
        {
            new UserIdentityRegisterRequest
            {
                Login = "user@gmail.com",
                UserPassword = new UserPassword
                {
                    Password = "@Test2023",
                    PasswordConfirm = "@Test2023"
                },
                RegistrationDate = DateTime.Now
            }
        };
    }

    [Theory]
    [Trait("Failed", "An error occurred while trying to register identity data")]
    [MemberData(nameof(UserIdentityForRegisterTheLoginIsAlreadyInTheDatabase))]
    public async Task CreateUserIdentityAsync_AnErrorOccuredWhileTryingToRegisterIdentityData_ReturnFalse(UserIdentityRegisterRequest accountIdentityRegisterRequest)
    {
        _userIdentityRepository.Setup(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserIdentity>())).ReturnsAsync(false);
        _userIdentityMapper.Setup(m => m.DtoUserIdentityRegisterRequestToDomain(It.IsAny<UserIdentityRegisterRequest>())).Returns(It.IsAny<UserIdentity>());
        _validate.Setup(v => v.ValidationAsync(It.IsAny<UserIdentity>())).ReturnsAsync(_validationResponse);
        _userIdentityRepository.Setup(a => a.SaveAsync(It.IsAny<UserIdentity>())).ReturnsAsync(IdentityResult.Failed());

        var serviceResult = await _userIdentityCommandService.CreateIdentityAccountAsync(accountIdentityRegisterRequest);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserIdentity>()), Times.Once());
        _userIdentityMapper.Verify(m => m.DtoUserIdentityRegisterRequestToDomain(It.IsAny<UserIdentityRegisterRequest>()), Times.Once());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Once());
        _userIdentityRepository.Verify(a => a.SaveAsync(It.IsAny<UserIdentity>()), Times.Once());
    }
}
