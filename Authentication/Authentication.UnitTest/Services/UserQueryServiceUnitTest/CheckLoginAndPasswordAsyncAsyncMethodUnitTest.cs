using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.UnitTest.Services.UserQueryServiceUnitTest.Base;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Authentication.UnitTest.Services.UserQueryServiceUnitTest;

public sealed class CheckLoginAndPasswordAsyncAsyncMethodUnitTest : UserIdentityQueryServiceSetup
{
    [Fact]
    [Trait("Query", "Exist")]
    public async Task CheckLoginAndPasswordAsyncAsync_Exist_ReturnTrue()
    {
        var userLogin = new AuthenticationRequest
        {
            Login = "loginTest@test.com",
            Password = "@Test2023",
            SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")  
        };

        UserIdentityRepository
            .Setup(r => r.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(SignInResult.Success);

        var serviceResult = await UserQueryService.CheckLoginAndPasswordAsync(userLogin);

        Assert.True(serviceResult);
        UserIdentityRepository
            .Verify(r => r.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once());
    }

    [Fact]
    [Trait("Query", "Not found")]
    public async Task CheckLoginAndPasswordAsyncAsync_NotFound_ReturnFalse()
    {
        var userLogin = new AuthenticationRequest
        {
            Login = "loginTest@test.com",
            Password = "@Test2023",
            SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")  
        };

        UserIdentityRepository
            .Setup(r => r.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(SignInResult.Failed);

        var serviceResult = await UserQueryService.CheckLoginAndPasswordAsync(userLogin);

        Assert.False(serviceResult);
        UserIdentityRepository
            .Verify(r => r.PasswordSignInAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>()), Times.Once());
    }
}