namespace Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
public sealed record UserIdentityChangePasswordRequest
{
    public required Guid UserIdentityId { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}
