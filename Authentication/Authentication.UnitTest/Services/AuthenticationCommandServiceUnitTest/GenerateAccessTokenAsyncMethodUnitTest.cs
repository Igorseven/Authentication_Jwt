using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest.Base;
using Moq;
using System.Security.Claims;
using Authentication.UnitTest.TestTools;

namespace Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest;
public sealed class GenerateAccessTokenAsyncMethodUnitTest : AuthenticationCommandServiceSetup
{
    [Fact]
    [Trait("Success", "Generate access token")]
    public async Task GenerateAccessTokenAsync_GenerateAccessToken_ReturnAuthenticationResponse()
    {
        AuthenticationRequest userLogin = new()
        {
            Login = "login@test.com",
            Password = "@Password2023",
            SystemOrigin = Guid.NewGuid()
        };

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userLogin.Login),
            new Claim(ClaimTypes.Actor, EUserType.Client.ToString())
        };

        UserIdentityQueryService
            .Setup(a => a.CheckLoginAndPasswordAsync(It.IsAny<AuthenticationRequest>()))
            .ReturnsAsync(true);
        RefreshTokenRepository
            .Setup(r => r.DeleteAsync(UtilTools.BuildPredicateFunc<UserToken>()))
            .ReturnsAsync(true);
        UserIdentityQueryService
            .Setup(a => a.GetUseClaimsAsync(It.IsAny<string>()))
            .ReturnsAsync(claims);
        RefreshTokenRepository
            .Setup(r => r.SaveAsync(It.IsAny<UserToken>()))
            .ReturnsAsync(true);

        var serviceResult = await AuthenticationCommandService.GenerateAccessTokenAsync(userLogin);

        Assert.NotNull(serviceResult);
        UserIdentityQueryService
            .Verify(a => a.CheckLoginAndPasswordAsync(
                It.IsAny<AuthenticationRequest>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.DeleteAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Once());
        UserIdentityQueryService
            .Verify(a => a.GetUseClaimsAsync(
                It.IsAny<string>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.SaveAsync(
                It.IsAny<UserToken>()), Times.Once());
    }


    [Fact]
    [Trait("Failed", "Invalid login or password")]
    public async Task GenerateAccessTokenAsync_InvalidLoginOrPassword_ReturnAuthenticationResponse()
    {
        AuthenticationRequest userLogin = new()
        {
            Login = "login@test.com",
            Password = "@Password2023",
            SystemOrigin = Guid.NewGuid()
        };

        UserIdentityQueryService
            .Setup(a => a.CheckLoginAndPasswordAsync(It.IsAny<AuthenticationRequest>()))
            .ReturnsAsync(false);

        var serviceResult = await AuthenticationCommandService.GenerateAccessTokenAsync(userLogin);

        Assert.Null(serviceResult);
        UserIdentityQueryService
            .Verify(a => a.CheckLoginAndPasswordAsync(
                It.IsAny<AuthenticationRequest>()), Times.Once());
        RefreshTokenRepository.
            Verify(r => r.DeleteAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Never());
        UserIdentityQueryService
            .Verify(a => a.FindByUserNameAsync(
                It.IsAny<string>()), Times.Never());
        UserIdentityQueryService
            .Verify(a => a.GetUseClaimsAsync(
                It.IsAny<string>()), Times.Never());
        RefreshTokenRepository
            .Verify(r => r.SaveAsync(
                It.IsAny<UserToken>()), Times.Never());
    }
}
