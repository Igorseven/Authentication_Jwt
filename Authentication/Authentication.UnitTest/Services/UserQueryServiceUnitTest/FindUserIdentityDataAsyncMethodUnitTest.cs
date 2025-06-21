using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.UserQueryServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;

namespace Authentication.UnitTest.Services.UserQueryServiceUnitTest;

public sealed class FindUserIdentityDataAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Return dto")]
    public async Task FindUserIdentityDataAsync_ReturnAccountIdentityDataResponse()
    {
        var dtoResponse = new UserSimpleResponse
        {
            Id = Guid.NewGuid(),
            UserStatus = EUserStatus.Active,
            UserType = EUserType.Client
        };
        
        const string userName = "testlogin@test.com";
        
        var accountIdentity = UserBuilderTest
            .NewObject()
            .DomainObject();

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                true))
            .ReturnsAsync(accountIdentity);
        UserIdentityMapper
            .Setup(m => m.DomainToSimpleResponse(It.IsAny<User>()))
            .Returns(dtoResponse);

        var serviceResult = await UserQueryService.FindByLoginAsync(userName);

        Assert.NotNull(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                true), Times.Once());
        UserIdentityMapper
            .Verify(m => m.DomainToSimpleResponse(
                It.IsAny<User>()), Times.Once());
    }

    [Fact]
    [Trait("Query", "Return null")]
    public async Task FindUserIdentityDataAsync_ReturnNull()
    {
        const string userName = "testlogin@test.com";

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                true));

        var serviceResult = await UserQueryService.FindByLoginAsync(userName);

        Assert.Null(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                true), Times.Once());
        UserIdentityMapper
            .Verify(m => m.DomainToSimpleResponse(
                It.IsAny<User>()), Times.Never());
    }
}