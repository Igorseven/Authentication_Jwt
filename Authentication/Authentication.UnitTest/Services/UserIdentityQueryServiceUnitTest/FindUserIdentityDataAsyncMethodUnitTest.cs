using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;

namespace Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest;
public sealed class FindUserIdentityDataAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Return dto")]
    public async Task FindUserIdentityDataAsync_ReturnAccountIdentityDataResponse()
    {
        var dtoResponse = new UserIdentityDataResponse
        {
            UserIdentityId = Guid.NewGuid(),
            UserStatus = EUserStatus.Active,
            UserType = EUserType.Client
        };
        string userName = "testlogin@test.com";
        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                              true)).ReturnsAsync(accountIdentity);
        _userIdentityMapper.Setup(m => m.DomainToDtoUserIdentityData(It.IsAny<UserIdentity>())).Returns(dtoResponse);

        var serviceResult = await _userIdentityQueryService.FindUserIdentityDataAsync(userName);

        Assert.NotNull(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               true), Times.Once());
        _userIdentityMapper.Verify(m => m.DomainToDtoUserIdentityData(It.IsAny<UserIdentity>()), Times.Once());
    }

    [Fact]
    [Trait("Query", "Return null")]
    public async Task FindUserIdentityDataAsync_ReturnNull()
    {
        string userName = "testlogin@test.com";

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                              true));

        var serviceResult = await _userIdentityQueryService.FindUserIdentityDataAsync(userName);

        Assert.Null(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               true), Times.Once());
        _userIdentityMapper.Verify(m => m.DomainToDtoUserIdentityData(It.IsAny<UserIdentity>()), Times.Never());
    }
}
