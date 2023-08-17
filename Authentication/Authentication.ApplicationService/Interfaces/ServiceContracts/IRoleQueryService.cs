using Authentication.ApplicationService.DataTransferObjects.Responses.RoleResponse;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IRoleQueryService
{
    Task<IEnumerable<RoleForPermissionsResponse>> FindAllRolesAsync();
}
