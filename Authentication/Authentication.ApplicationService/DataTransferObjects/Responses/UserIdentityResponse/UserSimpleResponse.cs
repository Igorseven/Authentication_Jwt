using Authentication.Domain.Enums;

namespace Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
public sealed record UserSimpleResponse
{
    public required Guid Id { get; init; }
    public EUserType UserType { get; init; }
    public EUserStatus UserStatus { get; init; }
}
