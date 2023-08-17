using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest.Base;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Authentication.UnitTest.Services.AccountIdentityQueryServiceUnitTest;
public sealed class CheckLoginAndPasswordAsyncAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Exist")]
    public async Task CheckLoginAndPasswordAsyncAsync_Exist_ReturnTrue()
    {
        var userLogin = new UserLogin
        {
            Login = "loginTest@test.com",
            Password ="@Test2023"
        };

        _userIdentityRepository.Setup(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(SignInResult.Success);

        var serviceResult = await _userIdentityQueryService.CheckLoginAndPasswordAsyncAsync(userLogin);

        Assert.True(serviceResult);
        _userIdentityRepository.Verify(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
    }

    [Fact]
    [Trait("Query", "Not found")]
    public async Task CheckLoginAndPasswordAsyncAsync_NotFound_ReturnFalse()
    {
        var userLogin = new UserLogin
        {
            Login = "loginTest@test.com",
            Password = "@Test2023"
        };

        _userIdentityRepository.Setup(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(SignInResult.Failed);

        var serviceResult = await _userIdentityQueryService.CheckLoginAndPasswordAsyncAsync(userLogin);

        Assert.False(serviceResult);
        _userIdentityRepository.Verify(r => r.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
    }
}
