namespace Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
public sealed record AuthenticationRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
    public required Guid SystemOrigin { get; init; }
}
