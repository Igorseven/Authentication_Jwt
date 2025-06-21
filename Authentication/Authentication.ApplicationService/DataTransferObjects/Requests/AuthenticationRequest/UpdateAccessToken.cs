namespace Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
public sealed record UpdateAccessToken
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required Guid SystemOrigin { get; init; }
}
