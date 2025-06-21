using Authentication.API.Extensions;
using Authentication.API.Settings.RolesPolicy;
using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.Domain.Handlers.NotificationHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly IUserQueryService _userQueryService;

    public UserController(
        IUserCommandService userCommandService,
        IUserQueryService userQueryService)
    {
        _userCommandService = userCommandService;
        _userQueryService = userQueryService;
    }

    [AllowAnonymous]
    [HttpPost("register_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> UserRegisterAsync(
        [FromBody] UserRegisterRequest accountRegisterRequest, CancellationToken cancellationToken) =>
        await _userCommandService.RegisterAsync(accountRegisterRequest);

    [HttpPut("change_password_user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<bool> UserChangePasswordAsync(
        [FromBody] UserIdentityChangePasswordRequest accountIdentityChangePasswordRequest) =>
        await _userCommandService.ChangePasswordAsync(accountIdentityChangePasswordRequest);

    [Authorize(Roles = $"{UsersPolicy.ClientRole}, {UsersPolicy.ManagerRole}")]
    [HttpGet("get_user_identity_data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UserSimpleResponse?> GetUserIdentityData() =>
        await _userQueryService.FindByLoginAsync(User.GetUserName());
}