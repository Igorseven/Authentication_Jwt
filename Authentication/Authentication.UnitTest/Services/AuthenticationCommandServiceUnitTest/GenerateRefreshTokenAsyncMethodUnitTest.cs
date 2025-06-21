using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest.Base;
using Authentication.UnitTest.TestTools;
using Moq;
using System.Security.Claims;

namespace Authentication.UnitTest.Services.AuthenticationCommandServiceUnitTest;
public sealed class GenerateRefreshTokenAsyncMethodUnitTest : AuthenticationCommandServiceSetup
{
    public static IEnumerable<object[]> UpdateAccessTokenDtoPerfectSetting()
    {
        var refreshToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=jadne53.>";

        yield return new object[]
        {
            new UpdateAccessToken
            {
                AccessToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=",
                RefreshToken = refreshToken,
                SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")          
            },


            new RefreshToken
            {
                RefreshTokenId = 1,
                Token = refreshToken,
                UserName = "test@gmail.com"
            }
        };
    }

    [Theory]
    [Trait("Success", "Generate refresh token")]
    [MemberData(nameof(UpdateAccessTokenDtoPerfectSetting))]
    public async Task GenerateRefreshTokenAsync_GenerateRefreshToken_ReturnNewToken(
        UpdateAccessToken updateAccessToken,
        RefreshToken refreshToken)
    {
        var login = "login@login.com";
        var id = Guid.NewGuid();
        
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, refreshToken.UserName),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, nameof(EUserType.Client))
        };

        UserQueryService
            .Setup(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()))
            .ReturnsAsync((login, id));
        RefreshTokenRepository
            .Setup(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserToken>()))
            .ReturnsAsync(true);
        RefreshTokenRepository
            .Setup(r => r.DeleteAsync(UtilTools.BuildPredicateFunc<UserToken>()))
            .ReturnsAsync(true);
        UserQueryService
            .Setup(a => a.GetUseClaimsAsync(It.IsAny<string>()))
            .ReturnsAsync(claims);
        RefreshTokenRepository
            .Setup(r => r.SaveAsync(It.IsAny<UserToken>()))
            .ReturnsAsync(true);

        var serviceResult = await AuthenticationCommandService.GenerateRefreshTokenAsync(updateAccessToken);

        Assert.True(serviceResult?.RefreshToken != refreshToken.Token);
        UserQueryService
            .Verify(a => a.ExtractUserFromAccessTokenAsync(
                It.IsAny<ExtractUserRequest>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.HaveInTheDatabaseAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Once());
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


    public static IEnumerable<object[]> UpdateAccessTokenDtoInvalidRefreshToken()
    {
        var refreshToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=jadne53.>";

        yield return new object[]
        {
            new UpdateAccessToken
            {
                AccessToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=",
                RefreshToken = refreshToken,
                SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")  
            }
        };
    }

    [Theory]
    [Trait("Failed", "Invalid refresh token")]
    [MemberData(nameof(UpdateAccessTokenDtoInvalidRefreshToken))]
    public async Task GenerateRefreshTokenAsync_InvalidRefreshToken_ReturnNullAndNotification(
        UpdateAccessToken updateAccessToken)
    {
        var login = "login@login.com";
        var id = Guid.NewGuid();
        
        UserQueryService
            .Setup(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()))
            .ReturnsAsync((login, id));
        RefreshTokenRepository
            .Setup(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<UserToken>()))
            .ReturnsAsync(false);


        var serviceResult = await AuthenticationCommandService.GenerateRefreshTokenAsync(updateAccessToken);

        Assert.Null(serviceResult);
        UserQueryService
            .Verify(a => a.ExtractUserFromAccessTokenAsync(
                It.IsAny<ExtractUserRequest>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.HaveInTheDatabaseAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.DeleteAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Never());
        UserQueryService
            .Verify(a => a.GetUseClaimsAsync(
                It.IsAny<string>()), Times.Never());
        RefreshTokenRepository
            .Verify(r => r.SaveAsync(
                It.IsAny<UserToken>()), Times.Never());
    }


    public static IEnumerable<object[]> UpdateAccessTokenDtoErrorExtractingTokenData()
    {
        var refreshToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=jadne53.>";

        yield return new object[]
        {
            new UpdateAccessToken
            {
                AccessToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=",
                RefreshToken = refreshToken,
                SystemOrigin = new Guid("8ad769d7-ab2e-4a91-a854-c831a8100009")  
            }
        };
    }

    [Theory]
    [Trait("Failed", "Error extracting token data")]
    [MemberData(nameof(UpdateAccessTokenDtoErrorExtractingTokenData))]
    public async Task GenerateRefreshTokenAsync_ErrorExtractingTokenData_ReturnNullAndNotification(
        UpdateAccessToken updateAccessToken)
    {
        UserQueryService
            .Setup(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()));

        var serviceResult = await AuthenticationCommandService.GenerateRefreshTokenAsync(updateAccessToken);

        Assert.Null(serviceResult);
        UserQueryService
            .Verify(a => a.ExtractUserFromAccessTokenAsync(
                It.IsAny<ExtractUserRequest>()), Times.Once());
        RefreshTokenRepository
            .Verify(r => r.HaveInTheDatabaseAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Never());
        RefreshTokenRepository
            .Verify(r => r.DeleteAsync(
                UtilTools.BuildPredicateFunc<UserToken>()), Times.Never());
        UserQueryService
            .Verify(a => a.GetUseClaimsAsync(
                It.IsAny<string>()), Times.Never());
        RefreshTokenRepository
            .Verify(r => r.SaveAsync(
                It.IsAny<UserToken>()), Times.Never());
    }
}
