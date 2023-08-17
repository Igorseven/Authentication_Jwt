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
                RefreshToken = refreshToken
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
    public async Task GenerateRefreshTokenAsync_GenerateRefreshToken_ReturnNewToken(UpdateAccessToken updateAccessToken, RefreshToken refreshToken)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, refreshToken.UserName),
            new Claim(ClaimTypes.Role, EUserType.Client.ToString())
        };

        var accountIdentity = new UserIdentity
        {
            Id = Guid.NewGuid(),
            UserName = refreshToken.UserName,
            PasswordHash = "@Password2023",
            UserStatus = EUserStatus.Active
        };

        _userIdentityQueryService.Setup(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>())).ReturnsAsync(refreshToken.UserName);
        _refreshTokenRepository.Setup(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<RefreshToken>())).ReturnsAsync(true);
        _refreshTokenRepository.Setup(r => r.DeleteAsync(refreshToken.UserName, refreshToken.Token)).ReturnsAsync(true);
        _userIdentityQueryService.Setup(a => a.GetUseClaimsAsync(It.IsAny<string>())).ReturnsAsync(claims);
        _refreshTokenRepository.Setup(r => r.SaveAsync(It.IsAny<RefreshToken>())).ReturnsAsync(true);

        var serviceResult = await _authenticationCommandService.GenerateRefreshTokenAsync(updateAccessToken);

        Assert.True(serviceResult?.RefreshToken != refreshToken.Token);
        _userIdentityQueryService.Verify(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<RefreshToken>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.DeleteAsync(refreshToken.UserName, refreshToken.Token), Times.Once());
        _userIdentityQueryService.Verify(a => a.GetUseClaimsAsync(It.IsAny<string>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.SaveAsync(It.IsAny<RefreshToken>()), Times.Once());
    }


    public static IEnumerable<object[]> UpdateAccessTokenDtoInvalidRefreshToken()
    {
        var refreshToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=jadne53.>";

        yield return new object[]
        {
            new UpdateAccessToken
            {
                AccessToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=",
                RefreshToken = refreshToken
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
    [Trait("Failed", "Invalid refresh token")]
    [MemberData(nameof(UpdateAccessTokenDtoInvalidRefreshToken))]
    public async Task GenerateRefreshTokenAsync_InvalidRedreshToken_ReturnNullAndNotification(UpdateAccessToken updateAccessToken, RefreshToken refreshToken)
    {
        _userIdentityQueryService.Setup(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>())).ReturnsAsync(refreshToken.UserName);
        _refreshTokenRepository.Setup(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<RefreshToken>())).ReturnsAsync(false);


        var serviceResult = await _authenticationCommandService.GenerateRefreshTokenAsync(updateAccessToken);

        Assert.Null(serviceResult);
        _userIdentityQueryService.Verify(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<RefreshToken>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.DeleteAsync(refreshToken.UserName, refreshToken.Token), Times.Never());
        _userIdentityQueryService.Verify(a => a.GetUseClaimsAsync(It.IsAny<string>()), Times.Never());
        _refreshTokenRepository.Verify(r => r.SaveAsync(It.IsAny<RefreshToken>()), Times.Never());
    }


    public static IEnumerable<object[]> UpdateAccessTokenDtoErrorExtractingTokenData()
    {
        var refreshToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=jadne53.>";

        yield return new object[]
        {
            new UpdateAccessToken
            {
                AccessToken = "C15Q4RluHecvYbJmtdPoGdc+hnOqD1p8OWP418mPJj8=",
                RefreshToken = refreshToken
            }
        };
    }

    [Theory]
    [Trait("Failed", "Error extracting token data")]
    [MemberData(nameof(UpdateAccessTokenDtoErrorExtractingTokenData))]
    public async Task GenerateRefreshTokenAsync_ErrorExtractingTokenData_ReturnNullAndNotification(UpdateAccessToken updateAccessToken)
    {
        _userIdentityQueryService.Setup(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()));

        var serviceResult = await _authenticationCommandService.GenerateRefreshTokenAsync(updateAccessToken);

        Assert.Null(serviceResult);
        _userIdentityQueryService.Verify(a => a.ExtractUserFromAccessTokenAsync(It.IsAny<ExtractUserRequest>()), Times.Once());
        _refreshTokenRepository.Verify(r => r.HaveInTheDatabaseAsync(UtilTools.BuildPredicateFunc<RefreshToken>()), Times.Never());
        _refreshTokenRepository.Verify(r => r.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        _userIdentityQueryService.Verify(a => a.GetUseClaimsAsync(It.IsAny<string>()), Times.Never());
        _refreshTokenRepository.Verify(r => r.SaveAsync(It.IsAny<RefreshToken>()), Times.Never());
    }
}
