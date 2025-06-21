namespace Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
public sealed record UserPassword
{
    public required string Password { get; init; }
    public required string PasswordConfirm { get; init; }
}
