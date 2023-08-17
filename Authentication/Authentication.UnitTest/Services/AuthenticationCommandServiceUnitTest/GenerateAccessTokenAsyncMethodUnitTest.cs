using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest.Base;
using Moq;
using System.Security.Claims;

namespace RegistrationManagement.UnitTest.Services.AuthenticationCommandServiceUnitTest;
public sealed class GenerateAccessTokenAsyncMethodUnitTest : AuthenticationCommandServiceSetup
{

    public static IEnumerable<object[]> AccountIdentyDomainObject()
    {
        yield return new object[]
        {
            new UserIdentity
            {
                Id = Guid.NewGuid(),
                UserName = "login@test.com",
                PasswordHash = "@Password2023",
                UserStatus = EUserStatus.Active
            }
        };
    }


    [Theory]
    [Trait("Success", "Generate access token")]
    [MemberData(nameof(AccountIdentyDomainObject))]
    public async Task GenerateAccessTokenAsync_GenerateAccessToken_ReturnAuthenticationResponse(UserIdentity userIdentity)
    {
        UserLogin userLogin = new()
        {
            Login = "login@test.com",
            Password = "@Password2023"
        };

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userLogin.Login),
            new Claim(ClaimTypes.Actor, EUserType.Client.ToString())
        };

        _userIdentityQueryService.Setup(a => a.CheckLoginAndPasswordAsyncAsync(It.IsAny<UserLogin>())).ReturnsAsync(true);
        _refreshTokenRepository.Setup(r => r.DeleteAsync(It.IsAny<string>())).ReturnsAsync(true);
        _userIdentityQueryService.Setup(a => a.GetUseClaimsAsync(It.IsAny<string>())).ReturnsAsync(claims);
        _refreshTokenRepository.Setup(r => r.SaveAsync(It.IsAny<RefreshToken>())).ReturnsAsync(true);

        var serviceResult = await _authenticationCommandService.GenerateAccessTokenAsync(userLogin);

        Assert.NotNull(serviceResult);
        _userIdentityQueryService.Verify(a => a.CheckLoginAndPasswordAsyncAsync(It.IsAny<UserLogin>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Once());
        _userIdentityQueryService.Verify(a => a.GetUseClaimsAsync(It.IsAny<string>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.SaveAsync(It.IsAny<RefreshToken>()), Times.Once());
    }


    [Fact]
    [Trait("Failed", "Invalid login or password")]
    public async Task GenerateAccessTokenAsync_InvalidLoginOrPassword_ReturnAuthenticationResponse()
    {
        UserLogin userLogin = new()
        {
            Login = "login@test.com",
            Password = "@Password2023"
        };

        _userIdentityQueryService.Setup(a => a.CheckLoginAndPasswordAsyncAsync(It.IsAny<UserLogin>())).ReturnsAsync(false);

        var serviceResult = await _authenticationCommandService.GenerateAccessTokenAsync(userLogin);

        Assert.Null(serviceResult);
        _userIdentityQueryService.Verify(a => a.CheckLoginAndPasswordAsyncAsync(It.IsAny<UserLogin>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never());
        _userIdentityQueryService.Verify(a => a.FindByUserNameAsync(It.IsAny<string>()), Times.Never());
        _userIdentityQueryService.Verify(a => a.GetUseClaimsAsync(It.IsAny<string>()), Times.Never());
        _refreshTokenRepository.Verify(r => r.SaveAsync(It.IsAny<RefreshToken>()), Times.Never());
    }
}
