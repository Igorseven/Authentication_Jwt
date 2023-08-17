using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.UserIdentityCommandServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace Authentication.UnitTest.Services.AccountIdentityCommandServiceUnitTest;
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
        _userIdentityRepository.Setup(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(SignInResult.Success);
        _validate.Setup(v => v.ValidationAsync(It.IsAny<UserIdentity>())).ReturnsAsync(_validationResponse);
        _userIdentityRepository.Setup(r => r.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.True(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Once());
        _userIdentityRepository.Verify(r => r.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()), Times.Once());
    }

    [Fact]
    [Trait("Failed", "Invalid data")]
    public async Task ChangePasswordAsync_InvalidData_ReturnFalse()
    {
        CreateInvalidDataNotification();
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
        _userIdentityRepository.Setup(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(SignInResult.Success);
        _validate.Setup(v => v.ValidationAsync(It.IsAny<UserIdentity>())).ReturnsAsync(_validationResponse);

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Once());
        _userIdentityRepository.Verify(r => r.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()), Times.Never());

    }

    [Fact]
    [Trait("Failed", "Invalid password")]
    public async Task ChangePasswordAsync_InvalidPassword_ReturnFalse()
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
        _userIdentityRepository.Setup(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(SignInResult.Failed);

        var serviceResult = await _userIdentityCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(r => r.FindByPredicateWithSelectorAsync(UtilTools.BuildPredicateFunc<UserIdentity>(),
                                                                               UtilTools.BuildSelectorFunc<UserIdentity>(),
                                                                               false), Times.Once());
        _userIdentityRepository.Verify(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Never());
        _userIdentityRepository.Verify(r => r.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()), Times.Never());
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
        _userIdentityRepository.Verify(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        _validate.Verify(v => v.ValidationAsync(It.IsAny<UserIdentity>()), Times.Never());
        _userIdentityRepository.Verify(r => r.ResetPasswordAsync(It.IsAny<UserIdentity>(), It.IsAny<string>()), Times.Never());
    }
}
