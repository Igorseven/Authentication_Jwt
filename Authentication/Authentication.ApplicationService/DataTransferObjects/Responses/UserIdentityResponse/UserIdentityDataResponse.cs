using Authentication.Domain.Enums;

namespace Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
public sealed record UserIdentityDataResponse
{
    public required Guid UserIdentityId { get; set; }
    public EUserType UserType { get; set; }
    public EUserStatus UserStatus { get; set; }
}
