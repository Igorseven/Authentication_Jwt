using Authentication.Domain.Entities;
using Authentication.Domain.Enums;

namespace Authentication.UnitTest.Builders;
public sealed class UserBuilderTest
{
    private Guid _id = Guid.NewGuid();
    private EUserType _userType = EUserType.Client;
    private EUserStatus _userStatus = EUserStatus.Inconsistent;
    private string _userName = "usertest@test.com";
    private string? _email;
    private string? _cellPhone;
    private string _password = "@Tester2023";

    public static UserBuilderTest NewObject() => new();

    public User DomainObject() =>
        new()
        {
            Id = _id,
            UserName = _userName,
            Status = _userStatus,
            Type = _userType,
            Email = _email,
            PhoneNumber = _cellPhone,
            PasswordHash = _password
        };

    public UserBuilderTest WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilderTest WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UserBuilderTest WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilderTest WithPhoneNumber(string cellPhone)
    {
        _cellPhone = cellPhone;
        return this;
    }

    public UserBuilderTest WithPasswordHash(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilderTest WithUserStatus(EUserStatus userStatus)
    {
        _userStatus = userStatus;
        return this;
    }

    public UserBuilderTest WithUserType(EUserType userType)
    {
        _userType = userType;
        return this;
    }
}
