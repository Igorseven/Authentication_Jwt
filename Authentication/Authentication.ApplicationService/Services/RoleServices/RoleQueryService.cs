using Authentication.ApplicationService.DataTransferObjects.Responses.RoleResponse;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using System.Linq.Expressions;

namespace Authentication.ApplicationService.Services.RoleServices;
public sealed class RoleQueryService : IRoleQueryService
{
    private readonly IRoleMapper _roleMapper;
    private readonly IRoleRepository _roleRepository;

    public RoleQueryService(IRoleMapper roleMapper, IRoleRepository roleRepository)
    {
        _roleMapper = roleMapper;
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleForPermissionsResponse>> FindAllRolesAsync()
    {
        var roles = await _roleRepository.FindAllAsync(QueryProjectionForFindAllRoles());

        return _roleMapper.DomainToDtoPermissionsGridResponses(roles);
    }

    private static Expression<Func<Role, Role>> QueryProjectionForFindAllRoles() =>
        r => new Role
        {
            Id = r.Id,
            Name = r.Name,
        };
}
