using Authentication.ApplicationService.DataTransferObjects.Requests.RoleRequest;

namespace Authentication.ApplicationService.Interfaces.ServiceContracts;
public interface IRoleCommandService : IDisposable
{
    Task<bool> CreateRoleAsync(RoleRegisterRequest roleRegisterRequest);
    Task<bool> DeleteRoleAsync(Guid roleId);
}
