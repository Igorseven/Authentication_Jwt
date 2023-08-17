namespace Authentication.ApplicationService.DataTransferObjects.Responses.RoleResponse;
public sealed record RoleForWithNumberOfUsersResponse
{
    public required Guid RoleId { get; set; }
    public required string RoleName { get; set; }
    public required int NumberOfUsers { get; set; }
}
