namespace Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;

public sealed record UserIdentityRegisterResponse
{
    public Guid? UserId { get; set; }
}