using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.UserIdentityCommandServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace Authentication.UnitTest.Services.UserIdentityCommandServiceUnitTest;
public sealed class ChangePasswordAsyncMethodUnitTest : UserIdentityCommandServiceSetup
{

    [Fact]
    [Trait("Success", "Perfect setting")]
    public async Task ChangePasswordAsync_PerfectSetting_ReturnTrue()
    {
        var dtoChangePassword = new UserIdentityChangePasswordRequest
        {
            UserIdentityId = Guid.NewGuid(),
            OldPassword = "@Test2022",
            NewPassword = "@Newtest2023"
        };


        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                              false)).ReturnsAsync(accountIdentity);
        _userIdentityRepository.Setup(r => r.ChangePasswordAsync(It.IsAny<UserIdentity>(),
                                                                 dtoChangePassword.OldPassword,
                                                                 dtoChangePassword.NewPassword)).ReturnsAsync(IdentityResult.Success);

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.True(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.ChangePasswordAsync(It.IsAny<UserIdentity>(),
                                                                  dtoChangePassword.OldPassword,
                                                                  dtoChangePassword.NewPassword), Times.Once());
    }

    [Fact]
    [Trait("Failed", "An error occurred while trying to persist the data")]
    public async Task ChangePasswordAsync_AnErrorOccurredWhileTryingToPersistTheData_ReturnFalse()
    {
        CreateInvalidDataNotification();
        var dtoChangePassword = new UserIdentityChangePasswordRequest
        {
            UserIdentityId = Guid.NewGuid(),
            OldPassword = "@Test2022",
            NewPassword = "@Test2023"
        };

        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                              false)).ReturnsAsync(accountIdentity);
        _userIdentityRepository.Setup(r => r.ChangePasswordAsync(It.IsAny<UserIdentity>(),
                                                                 dtoChangePassword.OldPassword,
                                                                 dtoChangePassword.NewPassword)).ReturnsAsync(IdentityResult.Failed());

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.ChangePasswordAsync(It.IsAny<UserIdentity>(),
                                                                  dtoChangePassword.OldPassword,
                                                                  dtoChangePassword.NewPassword), Times.Once());

    }

    [Fact]
    [Trait("Failed", "Invalid password")]
    public async Task ChangePasswordAsync_InvalidPassword_ReturnFalse()
    {
        var dtoChangePassword = new UserIdentityChangePasswordRequest
        {
            UserIdentityId = Guid.NewGuid(),
            OldPassword = "@Test2022",
            NewPassword = "test2023"
        };

        var accountIdentity = UserIdentityBuilderTest.NewObject().DomainObject();

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                              UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                              false)).ReturnsAsync(accountIdentity);

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.ChangePasswordAsync(It.IsAny<UserIdentity>(),
                                                                  dtoChangePassword.OldPassword,
                                                                  dtoChangePassword.NewPassword), Times.Never());
    }

    [Fact]
    [Trait("Failed", "User not found")]
    public async Task ChangePasswordAsync_UserNotFound_ReturnFalse()
    {
        var dtoChangePassword = new UserIdentityChangePasswordRequest
        {
            UserIdentityId = Guid.NewGuid(),
            OldPassword = "@Test2022",
            NewPassword = "@Newtest2023"
        };

        _userIdentityRepository.Setup(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                                      UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                                      false));

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.ChangePasswordAsync(It.IsAny<UserIdentity>(),
                                                                  dtoChangePassword.OldPassword,
                                                                  dtoChangePassword.NewPassword), Times.Never());
    }
}
