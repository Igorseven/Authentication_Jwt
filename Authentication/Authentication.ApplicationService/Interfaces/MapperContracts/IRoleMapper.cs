using Authentication.ApplicationService.DataTransferObjects.Requests.RoleRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.RoleResponse;
using Authentication.Domain.Entities;

namespace Authentication.ApplicationService.Interfaces.MapperContracts;
public interface IRoleMapper
{
    Role DtoRoleRegisterRequestToDomain(RoleRegisterRequest roleRegisterRequest);
    IEnumerable<RoleForPermissionsResponse> DomainToDtoPermissionsGridResponses(IEnumerable<Role> roles);
    RoleForPermissionsResponse DomainToDtoPermissionsResponse(Role role);
    RoleForWithNumberOfUsersResponse DomainToDtoWithNumberOfUsersResponse(Role role);
}
