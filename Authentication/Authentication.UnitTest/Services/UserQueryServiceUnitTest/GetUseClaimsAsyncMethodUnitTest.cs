using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.UserQueryServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;

namespace Authentication.UnitTest.Services.UserQueryServiceUnitTest;

public sealed class GetUseClaimsAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Return claims")]
    public async Task GetUseClaimsAsync_ReturnClaims()
    {
        var roles = new List<string>();
        const string userName = "userName";

        var accountIdentity = UserBuilderTest
            .NewObject()
            .DomainObject();

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                null,
                true))
            .ReturnsAsync(accountIdentity);
        UserIdentityRepository
            .Setup(r => r.FindAllRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(roles);

        var serviceResult = await UserQueryService.GetUseClaimsAsync(userName);

        Assert.NotNull(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                null,
                true), Times.Once());
        UserIdentityRepository
            .Verify(r => r.FindAllRolesAsync(
                It.IsAny<User>()), Times.Once());
    }
}