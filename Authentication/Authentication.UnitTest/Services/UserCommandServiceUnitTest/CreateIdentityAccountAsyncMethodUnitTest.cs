using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.Domain.Entities;
using Authentication.UnitTest.Services.UserCommandServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace Authentication.UnitTest.Services.UserIdentityCommandServiceUnitTest;
public sealed class CreateIdentityAccountAsyncMethodUnitTest : UserIdentityCommandServiceSetup
{
    public static IEnumerable<object[]> UserIdentityRegisterRequestPerfectSetting()
    {
        yield return new object[]
        {
            new UserRegisterRequest
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
    public async Task CreateUserIdentityAsync_PerfectSetting_ReturnTrue(UserRegisterRequest accountRegisterRequest)
    {
        UserIdentityRepository.Setup(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<User>())).ReturnsAsync(false);
        UserIdentityMapper.Setup(m => m.DtoRegisterToDomain(It.IsAny<UserRegisterRequest>())).Returns(It.IsAny<User>());
        Validate.Setup(v => v.ValidationAsync(It.IsAny<User>())).ReturnsAsync(ValidationResponse);
        UserIdentityRepository.Setup(a => a.SaveAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var serviceResult = await UserCommandService.RegisterAsync(accountRegisterRequest);

        Assert.True(serviceResult);
        UserIdentityRepository.Verify(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<User>()), Times.Once());
        UserIdentityMapper.Verify(m => m.DtoRegisterToDomain(It.IsAny<UserRegisterRequest>()), Times.Once());
        Validate.Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once());
        UserIdentityRepository.Verify(a => a.SaveAsync(It.IsAny<User>()), Times.Once());
    }


    public static IEnumerable<object[]> UserIdentityForRegisterTheLoginIsAlreadyInTheDatabase()
    {
        yield return new object[]
        {
            new UserRegisterRequest
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
    public async Task CreateUserIdentityAsync_PerfectSetting_ReturnFalse(UserRegisterRequest accountRegisterRequest)
    {
        UserIdentityRepository.Setup(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<User>())).ReturnsAsync(true);

        var serviceResult = await UserCommandService.RegisterAsync(accountRegisterRequest);

        Assert.False(serviceResult);
        UserIdentityRepository.Verify(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<User>()), Times.Once());
        UserIdentityMapper.Verify(m => m.DtoRegisterToDomain(It.IsAny<UserRegisterRequest>()), Times.Never());
        Validate.Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Never());
        UserIdentityRepository.Verify(a => a.SaveAsync(It.IsAny<User>()), Times.Never());
    }


    public static IEnumerable<object[]> UserIdentityForRegisterAnErrorOccuredWhileTryingToRegisterIdentityData()
    {
        yield return new object[]
        {
            new UserRegisterRequest
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
    public async Task CreateUserIdentityAsync_AnErrorOccuredWhileTryingToRegisterIdentityData_ReturnFalse(UserRegisterRequest accountRegisterRequest)
    {
        UserIdentityRepository.Setup(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<User>())).ReturnsAsync(false);
        UserIdentityMapper.Setup(m => m.DtoRegisterToDomain(It.IsAny<UserRegisterRequest>())).Returns(It.IsAny<User>());
        Validate.Setup(v => v.ValidationAsync(It.IsAny<User>())).ReturnsAsync(ValidationResponse);
        UserIdentityRepository.Setup(a => a.SaveAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed());

        var serviceResult = await UserCommandService.RegisterAsync(accountRegisterRequest);

        Assert.False(serviceResult);
        UserIdentityRepository.Verify(a => a.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<User>()), Times.Once());
        UserIdentityMapper.Verify(m => m.DtoRegisterToDomain(It.IsAny<UserRegisterRequest>()), Times.Once());
        Validate.Verify(v => v.ValidationAsync(It.IsAny<User>()), Times.Once());
        UserIdentityRepository.Verify(a => a.SaveAsync(It.IsAny<User>()), Times.Once());
    }
}
