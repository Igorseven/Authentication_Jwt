using Authentication.API.Extensions;
using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.Domain.Handlers.NotificationHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserIdentityController : ControllerBase
{
    private readonly IUserIdentityCommandService _userIdentityCommandService;
    private readonly IUserIdentityQueryService _userIdentityQueryService;

    public UserIdentityController(IUserIdentityCommandService userIdentityCommandService,
                                  IUserIdentityQueryService userIdentityQueryService)
    {
        _userIdentityCommandService = userIdentityCommandService;
        _userIdentityQueryService = userIdentityQueryService;
    }

    [AllowAnonymous]
    [HttpPost("user_identity_register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> UserRegisterAsync([FromBody] UserIdentityRegisterRequest accountIdentityRegisterRequest) =>
        await _userIdentityCommandService.CreateIdentityAccountAsync(accountIdentityRegisterRequest);

    [HttpPost("user_identity_change_password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> UserChangePasswordAsync([FromBody] UserIdentityChangePasswordRequest accountIdentityChangePasswordRequest) =>
        await _userIdentityCommandService.ChangePasswordAsync(accountIdentityChangePasswordRequest);

    [HttpGet("get_user_identity_data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UserIdentityDataResponse?> GetUserIdentityData() =>
        await _userIdentityQueryService.FindUserIdentityDataAsync(User.GetUserName());

}
