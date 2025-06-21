using Authentication.Domain.EntitiesValidations;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Builders;
using Bogus.Extensions;


namespace Authentication.UnitTest.Validations;

public sealed class UserValidationUnitTest
{
    private readonly UserValidation _validation;

    public UserValidationUnitTest()
    {
        _validation = new UserValidation();
    }

    [Fact]
    [Trait("Success", "Successful validation with all data")]
    public async Task UserIdentityValidation_ValidWithAllData_ReturnTrue()
    {
        const string login = "loginUserName@gmail.com";
        const string cellPhoneForAuth = "11910731189";
        const string password = "@Tester2023";
        const EUserType userType = EUserType.Client;
        const EUserStatus userStatus = EUserStatus.Active;


        var accountIdentity = UserBuilderTest
            .NewObject()
            .WithUserName(login)
            .WithPhoneNumber(cellPhoneForAuth)
            .WithUserType(userType)
            .WithUserStatus(userStatus)
            .WithPasswordHash(password)
            .DomainObject();

        var validationResponse = await _validation.ValidationAsync(accountIdentity);

        Assert.True(validationResponse.Valid);
    }


    [Fact]
    [Trait("Success", "Successful validation with partial data")]
    public async Task UserIdentityValidation_ValidWithPartialData_ReturnTrue()
    {
        const string login = "loginUserName@gmail.com";
        const string password = "@Tester2023";
        const EUserType userType = EUserType.Client;
        const EUserStatus userStatus = EUserStatus.Active;


        var accountIdentity = UserBuilderTest
            .NewObject()
            .WithUserName(login)
            .WithUserType(userType)
            .WithUserStatus(userStatus)
            .WithPasswordHash(password)
            .DomainObject();

        var validationResponse = await _validation.ValidationAsync(accountIdentity);

        Assert.True(validationResponse.Valid);
    }


    public static IEnumerable<object[]> InvalidUserNameOrLogin()
    {
        return new List<object[]>
        {
            new object[] { new Bogus.Faker().Internet.Email().ClampLength(1, 7) },
            new object[] { new Bogus.Faker().Internet.Email().ClampLength(151, 200) },
            new object[] { "email.gmail.com" },
            new object[] { "email.gmail.com.br" },
        };
    }

    [Theory]
    [Trait("Failed", "Invalid user name / login")]
    [MemberData(nameof(InvalidUserNameOrLogin))]
    public async Task UserIdentityValidation_InvalidUserNameOrLogin_ReturnFalse(string userNameLogin)
    {
        const string password = "@Tester2023";
        const EUserType userType = EUserType.Client;
        const EUserStatus userStatus = EUserStatus.Active;


        var accountIdentity = UserBuilderTest
            .NewObject()
            .WithUserName(userNameLogin)
            .WithUserType(userType)
            .WithUserStatus(userStatus)
            .WithPasswordHash(password)
            .DomainObject();

        var validationResponse = await _validation.ValidationAsync(accountIdentity);

        Assert.False(validationResponse.Valid);
    }


    public static IEnumerable<object[]> InvalidPassword()
    {
        return new List<object[]>
        {
            new object[] { "@test4500" },
            new object[] { "test4500" },
            new object[] { "@IGOR4500" },
            new object[] { "@1564500" },
            new object[] { "91564500" },
            new object[] { "TesterHen" },
            new object[] { "@#$%&*-+" },
        };
    }

    [Theory]
    [Trait("Failed", "Invalid cell phone")]
    [MemberData(nameof(InvalidPassword))]
    public async Task UserIdentityValidation_InvalidPassword_ReturnFalse(string password)
    {
        const string login = "loginUserName@gmail.com";
        const EUserType userType = EUserType.Client;
        const EUserStatus userStatus = EUserStatus.Active;


        var accountIdentity = UserBuilderTest
            .NewObject()
            .WithUserName(login)
            .WithUserType(userType)
            .WithUserStatus(userStatus)
            .WithPasswordHash(password)
            .DomainObject();

        var validationResponse = await _validation.ValidationAsync(accountIdentity);

        Assert.False(validationResponse.Valid);
    }
}