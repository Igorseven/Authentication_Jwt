using Authentication.ApplicationService.Mappers;

namespace Authentication.UnitTest.Mappers.UserIdentityMapperUnitTest.Base;
public abstract class UserIdentityMapperSetup
{
    protected readonly UserIdentityMapper _accountIdentityMapper;

    public UserIdentityMapperSetup()
    {
        _accountIdentityMapper = new();
    }
}
