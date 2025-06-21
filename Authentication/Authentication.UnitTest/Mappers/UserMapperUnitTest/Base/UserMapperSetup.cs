using Authentication.ApplicationService.Mappers;

namespace Authentication.UnitTest.Mappers.UserMapperUnitTest.Base;
public abstract class UserMapperSetup
{
    protected readonly UserMapper AccountMapper;

    protected UserMapperSetup()
    {
        AccountMapper = new UserMapper();
    }
}
