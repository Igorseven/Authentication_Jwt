using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;

namespace Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest;
public sealed class GetUseClaimsAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Return claims")]
    public async Task GetUseClaimsAsync_ReturnClains()
    {
        var roles = new List<string>();
        var userName = "userName";
        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();
        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              null,
                                                                              true)).ReturnsAsync(accountIdentity);
        _userIdentityRepository.Setup(r => r.FindAllRolesAsync(It.IsAny<UserIdentity>())).ReturnsAsync(roles);

        var serviceResult = await _userIdentityQueryService.GetUseClaimsAsync(userName);
       
        Assert.NotNull(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               null,
                                                                               true), Times.Once());
        _userIdentityRepository.Verify(r => r.FindAllRolesAsync(It.IsAny<UserIdentity>()), Times.Once());
    }
}
