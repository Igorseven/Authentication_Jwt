using Authentication.ApplicationService.DataTransferObjects.Requests.RoleRequest;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.ApplicationService.Interfaces.ServiceContracts;
using Authentication.ApplicationService.NotificatioTrace;
using Authentication.ApplicationService.Services.Base;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.Domain.Extensions;
using Authentication.Domain.Handlers.NotificationHandler;
using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Authentication.ApplicationService.Services.RoleServices;
public sealed class RoleCommandService : BaseService<Role>, IRoleCommandService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IRoleMapper _roleMapper;

    public RoleCommandService(INotificationHandler notification,
                              IValidate<Role> validate,
                              IRoleRepository roleRepository,
                              IRoleMapper roleMapper)
        : base(notification, validate)
    {
        _roleRepository = roleRepository;
        _roleMapper = roleMapper;
    }

    public void Dispose() => _roleRepository.Dispose();

    public async Task<bool> CreateRoleAsync(RoleRegisterRequest roleRegisterRequest)
    {
        var role = _roleMapper.DtoRoleRegisterRequestToDomain(roleRegisterRequest);

        if (!await EntityValidationAsync(role)) return false;

        if (await _roleRepository.HaveInDatabaseAsync(r => r.NormalizedName == role.NormalizedName))
            return _notification.CreateNotification(RoleServicesTrace.CreateRoleMethod, EMessage.Exist.GetDescription().FormatTo("Role"));

        var repositoryResult = await _roleRepository.SaveAsync(role);

        if (!repositoryResult.Succeeded) AddIdentityErrors(repositoryResult);

        return repositoryResult.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        var role = await _roleRepository.FindByPredicateWithSelectorAsync(r => r.Id == roleId, QueryProjectionForDelete(), false);

        if (role is null)
            return _notification.CreateNotification(RoleServicesTrace.DeleteRoleMethod, EMessage.NotFound.GetDescription().FormatTo("Role"));

        if (role.UserRoles!.Any())
            return _notification.CreateNotification(RoleServicesTrace.DeleteRoleMethod, "Não é possível excluir permissões com usuários vinculados.");

        var repositoryResult = await _roleRepository.DeleteAsync(role);

        if (!repositoryResult.Succeeded) AddIdentityErrors(repositoryResult);

        return repositoryResult.Succeeded;
    }


    private void AddIdentityErrors(IdentityResult identityResult)
    {
        foreach (var error in identityResult.Errors)
        {
            _notification.CreateNotification(new DomainNotification(RoleServicesTrace.RoleValidate, error.Description));
        }
    }


    private static Expression<Func<Role, Role>> QueryProjectionForDelete() =>
        role => new Role
        {
            Id = role.Id,
            Name = role.Name,
            UserRoles = role.UserRoles!.Select(userRole => new UserRole
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId
            }).ToList()
        };
}

