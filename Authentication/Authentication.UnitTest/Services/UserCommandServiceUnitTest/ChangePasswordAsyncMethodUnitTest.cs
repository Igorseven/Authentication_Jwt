using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.Domain.Entities;
using Authentication.UnitTest.Builders;
using Authentication.UnitTest.Services.UserCommandServiceUnitTest.Base;
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


        var user = UserBuilderTest
            .NewObject()
            .DomainObject();

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false))
            .ReturnsAsync(user);
        UserIdentityRepository
            .Setup(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
                dtoChangePassword.OldPassword,
                dtoChangePassword.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        var serviceResult = await UserCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.True(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false), Times.Once());
        UserIdentityRepository
            .Verify(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
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

        var accountIdentity = UserBuilderTest.NewObject().DomainObject();

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false))
            .ReturnsAsync(accountIdentity);
        UserIdentityRepository
            .Setup(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
                dtoChangePassword.OldPassword,
                dtoChangePassword.NewPassword))
            .ReturnsAsync(IdentityResult.Failed());

        var serviceResult = await UserCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false), Times.Once());
        UserIdentityRepository
            .Verify(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
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

        var accountIdentity = UserBuilderTest
            .NewObject()
            .DomainObject();

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false))
            .ReturnsAsync(accountIdentity);

        var serviceResult = await UserCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false), Times.Once());
        UserIdentityRepository
            .Verify(r => r.ChangePasswordAsync(
                It.IsAny<User>(),
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

        UserIdentityRepository
            .Setup(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false));

        var serviceResult = await UserCommandService.ChangePasswordAsync(dtoChangePassword);

        Assert.False(serviceResult);
        UserIdentityRepository
            .Verify(r => r.FindByPredicateWithSelectorAsync(
                UtilTools.BuildPredicateFunc<User>(),
                UtilTools.BuildSelectorFunc<User>(),
                false), Times.Once());
        UserIdentityRepository
            .Verify(r => r.ChangePasswordAsync(It.IsAny<User>(),
                dtoChangePassword.OldPassword,
                dtoChangePassword.NewPassword), Times.Never());
    }
}