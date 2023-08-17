namespace Authentication.ApplicationService.DataTransferObjects.Responses.RoleResponse;
public sealed record RoleForPermissionsResponse
{
    public required Guid RoleId { get; set; }
    public required string RoleName { get; set; }
}
