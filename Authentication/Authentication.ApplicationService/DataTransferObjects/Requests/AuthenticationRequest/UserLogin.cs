namespace Authentication.ApplicationService.DataTransferObjects.Requests.AuthenticationRequest;
public sealed record UserLogin
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}
