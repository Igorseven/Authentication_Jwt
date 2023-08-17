using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;

namespace Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest;
public sealed class FindByUserNameAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Return domain object")]
    public async Task FindByUserNameAsync_ReturnUserIdentity()
    {

        string userName = "testlogin@test.com";
        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              null,
                                                                              true)).ReturnsAsync(accountIdentity);

        var serviceResult = await _userIdentityQueryService.FindByUserNameAsync(userName);

        Assert.NotNull(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               null,
                                                                               true), Times.Once());
    }

    [Fact]
    [Trait("Query", "Return null")]
    public async Task FindByUserNameAsync_ReturnNull()
    {
        string userName = "testlogin@test.com";
        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              null,
                                                                              true));

        var serviceResult = await _userIdentityQueryService.FindByUserNameAsync(userName);

        Assert.Null(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               null,
                                                                               true), Times.Once());
    }
}
