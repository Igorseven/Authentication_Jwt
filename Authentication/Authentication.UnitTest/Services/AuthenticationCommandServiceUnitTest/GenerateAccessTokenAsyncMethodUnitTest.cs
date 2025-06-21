using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest.Base;
using Moq;
using System.Security.Claims;
using Authentication.UnitTest.Builders;
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
            SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")  
        };

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userLogin.Login),
            new Claim(ClaimTypes.Actor, nameof(EUserType.Client))
        };
        
        var user = UserBuilderTest
            .NewObject()
            .DomainObject();

        UserQueryService
            .Setup(a => a.FindByUserNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        RefreshTokenRepository
            .Setup(r => r.DeleteAsync(UtilTools.BuildPredicateFunc<UserToken>()))
            .ReturnsAsync(true);
        UserQueryService
            .Setup(a => a.GetUseClaimsAsync(It.IsAny<string>()))
            .ReturnsAsync(claims);
        RefreshTokenRepository
            .Setup(r => r.SaveAsync(It.IsAny<UserToken>()))
            .ReturnsAsync(true);

        var serviceResult = await AuthenticationCommandService.GenerateAccessTokenAsync(userLogin);

        Assert.NotNull(serviceResult);
        UserQueryService
            .Verify(a => a.FindByUserNameAsync(
                It.IsAny<string>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.DeleteAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Once());
        UserQueryService
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
            SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")  
        };

        UserQueryService
            .Setup(a => a.FindByUserNameAsync(It.IsAny<string>()));

        var serviceResult = await AuthenticationCommandService.GenerateAccessTokenAsync(userLogin);

        Assert.Null(serviceResult);
        UserQueryService
            .Verify(a => a.FindByUserNameAsync(
                It.IsAny<string>()), Times.Once());
        RefreshTokenRepository.
            Verify(r => r.DeleteAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Never());
        UserQueryService
            .Verify(a => a.GetUseClaimsAsync(
                It.IsAny<string>()), Times.Never());
        RefreshTokenRepository
            .Verify(r => r.SaveAsync(
                It.IsAny<UserToken>()), Times.Never());
    }
}
