using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.UserQueryServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;

namespace Authentication.UnitTest.Services.UserQueryServiceUnitTest;

public sealed class FindByUserNameAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Return domain object")]
    public async Task FindByUserNameAsync_ReturnUserIdentity()
    {
        const string userName = "testlogin@test.com";

        var accountIdentity = UserBuilderTest
            .NewObject()
            .DomainObject();

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                null,
                true))
            .ReturnsAsync(accountIdentity);

        var serviceResult = await UserQueryService.FindByUserNameAsync(userName);

        Assert.NotNull(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                null,
                true), Times.Once());
    }

    [Fact]
    [Trait("Query", "Return null")]
    public async Task FindByUserNameAsync_ReturnNull()
    {
        const string userName = "testlogin@test.com";

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                null,
                true));

        var serviceResult = await UserQueryService.FindByUserNameAsync(userName);

        Assert.Null(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                null,
                true), Times.Once());
    }
}