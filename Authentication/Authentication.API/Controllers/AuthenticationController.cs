using Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.AuthenticationResponse;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.Domain.Handlers.NotificationHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationCommandService _authenticationTokenCommandService;

    public AuthenticationController(IAuthenticationCommandService authenticationTokenCommandService)
    {
        _authenticationTokenCommandService = authenticationTokenCommandService;
    }

    [AllowAnonymous]
    [HttpPost("generate_access_token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    public async Task<AuthenticationLoginResponse> CreateAccessTokenAsync([FromBody] UserLogin userLogin) =>
        await _authenticationTokenCommandService.GenerateAccessTokenAsync(userLogin);

    [HttpPost("update_access_token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IEnumerable<DomainNotification>))]
    public async Task<AuthenticationLoginResponse?> CreateRefreshTokenAsync([FromBody] UpdateAccessToken updateAccessToken) =>
        await _authenticationTokenCommandService.GenerateRefreshTokenAsync(updateAccessToken);
}

