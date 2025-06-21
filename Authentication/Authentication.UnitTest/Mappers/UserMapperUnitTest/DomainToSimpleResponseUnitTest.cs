using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Mappers.UserMapperUnitTest.Base;

namespace Authentication.UnitTest.Mappers.UserMapperUnitTest;
public sealed class DomainToSimpleResponseUnitTest : UserMapperSetup
{
    [Fact]
    [Trait("Mapping", "Domain to dto identity data")]
    public void DomainToDtoSimpleResponse_ReturnDtoUserSimpleResponse()
    {
        var user = UserBuilderTest.NewObject().DomainObject();

        var mappingResult = AccountMapper.DomainToSimpleResponse(user);

        Assert.NotNull(mappingResult);
        Assert.Equal(user.Id, mappingResult.Id);
        Assert.Equal(user.Status, mappingResult.UserStatus);
        Assert.Equal(user.Type, mappingResult.UserType);
    }
}
