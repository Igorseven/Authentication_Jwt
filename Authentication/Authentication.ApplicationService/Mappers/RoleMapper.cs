using Authentication.ApplicationService.DataTransferObjects.Requests.RoleRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.RoleResponse;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.Domain.Entities;

namespace Authentication.ApplicationService.Mappers;
public sealed class RoleMapper : IRoleMapper
{
    public Role DtoRoleRegisterRequestToDomain(RoleRegisterRequest roleRegisterRequest) =>
        new()
        {
            Name = roleRegisterRequest.Name,
            NormalizedName = roleRegisterRequest.Name.ToUpper(),
            UserRoles = new List<UserRole>()
        };


    public IEnumerable<RoleForPermissionsResponse> DomainToDtoPermissionsGridResponses(IEnumerable<Role> roles)
    {
        if (!roles.Any()) return Enumerable.Empty<RoleForPermissionsResponse>();

        List<RoleForPermissionsResponse> roleForPermissionsGridResponses = new();

        foreach (var role in roles)
        {
            var response = DomainToDtoPermissionsResponse(role);

            roleForPermissionsGridResponses.Add(response);
        }

        return roleForPermissionsGridResponses;
    }

    public RoleForPermissionsResponse DomainToDtoPermissionsResponse(Role role) =>
        new()
        {
            RoleId = role.Id,
            RoleName = role.Name!
        };


    public RoleForWithNumberOfUsersResponse DomainToDtoWithNumberOfUsersResponse(Role role) =>
        new()
        {
            RoleId = role.Id,
            RoleName = role.Name!,
            NumberOfUsers = role.UserRoles!.Count
        };
}