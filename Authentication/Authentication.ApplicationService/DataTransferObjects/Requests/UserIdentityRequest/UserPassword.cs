namespace Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
public sealed record UserPassword
{
    public required string Password { get; init; }
    public required string PasswordConfirm { get; init; }
}
