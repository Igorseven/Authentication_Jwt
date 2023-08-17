using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Mappers.UserIdentityMapperUnitTest.Base;

namespace Authentication.UnitTest.Mappers.UserIdentityMapperUnitTest;
public sealed class DomainToDtoUserIdentityDataMethodUnitTest : UserIdentityMapperSetup
{
    [Fact]
    [Trait("Mapping", "Domain to dto identity data")]
    public void DomainToDtoUserIdentityData_ReturnDtoAccountIdentityDataResponse()
    {
        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        var mappingResult = _accountIdentityMapper.DomainToDtoUserIdentityData(accountIdentity);

        Assert.NotNull(mappingResult);
        Assert.Equal(accountIdentity.Id, mappingResult.UserIdentityId);
        Assert.Equal(accountIdentity.UserStatus, mappingResult.UserStatus);
        Assert.Equal(accountIdentity.UserType, mappingResult.UserType);
    }
}
