namespace Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
public sealed record UserIdentityChangePasswordRequest
{
    public required Guid UserIdentityId { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}
