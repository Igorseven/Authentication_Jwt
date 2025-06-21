using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.IntegrationTest.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.IntegrationTest.EndPointTests.UserIdentityIntegrationTests;

public sealed class UserChangePasswordAsyncMethodTest : BaseIntegrationTest
{
    private readonly IUserIdentityRepository _userIdentityRepository;
    private const string EndPointPostUrl = "api/UserIdentity/user_identity_register";
    private const string EndPointPutUrl = "api/UserIdentity/user_identity_change_password";

    public UserChangePasswordAsyncMethodTest(
        IntegrationTestWebAppFactory factory) 
        : base(factory)
    {
        _userIdentityRepository = _scope.ServiceProvider.GetRequiredService<IUserIdentityRepository>();
    }

    [Fact]
    [Trait("StatusCode 200", "Change password")]
    public async Task UserChangePasswordAsync_ReturnStatusCodeOk()
    {
        var dtoRegister = new UserRegisterRequest
        {
            Login = "tester@tester.com",
            UserPassword = new()
            {
                Password = "@Test2020",
                PasswordConfirm = "@Test2020"
            },
            RegistrationDate = DateTime.Now
        };
        
        await _client.PostAsJsonAsync(EndPointPostUrl, dtoRegister, JsonSerializerOptions.Default);

        var user = await _userIdentityRepository.FindByPredicateWithSelectorAsync(
            u => u.NormalizedUserName == dtoRegister.Login.ToUpper(),
            null,
            true);
        
        var dtoChangePassword = new UserIdentityChangePasswordRequest
        {
            UserIdentityId = user!.Id,
            NewPassword = "@Test2023",
            OldPassword = "@Test2023"
        };

        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

        var httpResponse = await _client.PutAsJsonAsync(EndPointPutUrl, dtoChangePassword);

        Assert.Equal(expectedStatusCode, httpResponse.StatusCode);
    }
}