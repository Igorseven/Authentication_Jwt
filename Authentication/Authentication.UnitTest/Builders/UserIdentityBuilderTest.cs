using Authentication.Domain.Entities;
using Authentication.Domain.Enums;

namespace Authentication.UnitTest.Builders;
public sealed class UserIdentityBuilderTest
{
    private Guid _id = Guid.NewGuid();
    private EUserType _userType = EUserType.Client;
    private EUserStatus _userStatus = EUserStatus.Inconsistent;
    private string _userName = "usertest@test.com";
    private string? _email;
    private string? _cellPhone;
    private string _password = "@Tester2023";

    public static UserIdentityBuilderTest NewObject() => new();

    public UserIdentity DomainObject() =>
        new()
        {
            Id = _id,
            UserName = _userName,
            UserStatus = _userStatus,
            UserType = _userType,
            Email = _email,
            PhoneNumber = _cellPhone,
            PasswordHash = _password
        };

    public UserIdentityBuilderTest WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserIdentityBuilderTest WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public UserIdentityBuilderTest WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserIdentityBuilderTest WithPhoneNumber(string cellPhone)
    {
        _cellPhone = cellPhone;
        return this;
    }

    public UserIdentityBuilderTest WithPasswordHash(string password)
    {
        _password = password;
        return this;
    }

    public UserIdentityBuilderTest WithUserStatus(EUserStatus userStatus)
    {
        _userStatus = userStatus;
        return this;
    }

    public UserIdentityBuilderTest WithUserType(EUserType userType)
    {
        _userType = userType;
        return this;
    }
}
