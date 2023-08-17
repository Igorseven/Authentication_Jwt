namespace Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
public sealed record UpdateAccessToken
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
