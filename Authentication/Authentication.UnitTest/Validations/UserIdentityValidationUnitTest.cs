using Authentication.Domain.EntitiesValidations;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Builders;
using Bogus.Extensions;


namespace Authentication.UnitTest.Validations;
public sealed class UserIdentityValidationUnitTest
{
    private readonly UserIdentityValidation _validation;

    public UserIdentityValidationUnitTest()
    {
        _validation = new();
    }

    [Fact]
    [Trait("Success", "Successful validation with all data")]
    public async Task UserIdentityValidation_ValidWithAllData_ReturnTrue()
    {
        var login = "loginUserName@gmail.com";
        var cellPhoneForAuth = "11910731189";
        var password = "@Tester2023";
        var userType = EUserType.Client;
        var userStatus = EUserStatus.Active;


        var accountIdentity = UserIdentityBuilderTest.NewObject()
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
        var login = "loginUserName@gmail.com";
        var password = "@Tester2023";
        var userType = EUserType.Client;
        var userStatus = EUserStatus.Active;


        var accountIdentity = UserIdentityBuilderTest.NewObject()
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
            new object[] {new Bogus.Faker().Internet.Email().ClampLength(1, 7)},
            new object[] {new Bogus.Faker().Internet.Email().ClampLength(151, 200)},
            new object[] {"email.gmail.com"},
            new object[] {"email.gmail.com.br"},
        };
    }

    [Theory]
    [Trait("Failed", "Invalid user name / login")]
    [MemberData(nameof(InvalidUserNameOrLogin))]
    public async Task UserIdentityValidation_InvalidUserNameOrLogin_ReturnFalse(string userNameLogin)
    {
        var password = "@Tester2023";
        var userType = EUserType.Client;
        var userStatus = EUserStatus.Active;


        var accountIdentity = UserIdentityBuilderTest.NewObject()
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
            new object[] {"@test4500"},
            new object[] {"test4500"},
            new object[] {"@IGOR4500"},
            new object[] {"@1564500"},
            new object[] {"91564500"},
            new object[] {"TesterHen"},
            new object[] {"@#$%&*-+"},
        };
    }

    [Theory]
    [Trait("Failed", "Invalid cell phone")]
    [MemberData(nameof(InvalidPassword))]
    public async Task UserIdentityValidation_InvalidPassword_ReturnFalse(string password)
    {
        var login = "loginUserName@gmail.com";
        var userType = EUserType.Client;
        var userStatus = EUserStatus.Active;


        var accountIdentity = UserIdentityBuilderTest.NewObject()
                                                     .WithUserName(login)
                                                     .WithUserType(userType)
                                                     .WithUserStatus(userStatus)
                                                     .WithPasswordHash(password)
                                                     .DomainObject();

        var validationResponse = await _validation.ValidationAsync(accountIdentity);

        Assert.False(validationResponse.Valid);
    }
}
