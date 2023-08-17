namespace Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
public sealed record UserIdentityRegisterRequest
{
    public required string Login { get; init; }
    public required UserPassword UserPassword { get; init; }
    public DateTime RegistrationDate { get; set; }
}
