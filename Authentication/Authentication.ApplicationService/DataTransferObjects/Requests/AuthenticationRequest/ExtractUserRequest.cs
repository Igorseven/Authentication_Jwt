using Microsoft.IdentityModel.Tokens;

namespace Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
public sealed record ExtractUserRequest
{
    public required string AccessToken { get; init; }
    public required string SecurityAlgorithm { get; init; }
    public required TokenValidationParameters TokenValidationParameters { get; init; }
}
