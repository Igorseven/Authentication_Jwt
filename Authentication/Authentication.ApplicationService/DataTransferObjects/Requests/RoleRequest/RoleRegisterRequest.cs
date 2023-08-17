namespace Authentication.ApplicationService.DataTransferObjects.Requests.RoleRequest;
public sealed record RoleRegisterRequest
{
    public required string Name { get; set; }
}
