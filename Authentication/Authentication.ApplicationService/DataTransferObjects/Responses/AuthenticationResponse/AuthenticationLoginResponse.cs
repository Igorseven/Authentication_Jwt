namespace Authentication.ApplicationService.DataTransferObjects.Responses.AuthenticationResponse;
public sealed record AuthenticationLoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required double Expiry { get; set; }
    public required string Message { get; set; }
}
