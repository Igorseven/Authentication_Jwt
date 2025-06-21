namespace Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
public sealed record UserRegisterRequest
{
    public required string Login { get; init; }
    public required UserPassword UserPassword { get; init; }
    public DateTime RegistrationDate { get; set; }
}
