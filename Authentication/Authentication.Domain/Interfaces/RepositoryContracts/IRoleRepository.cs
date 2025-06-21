using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Authentication.Domain.Interfaces.RepositoryContracts;

public interface IRoleRepository : IDisposable
{
    Task<IdentityResult> SaveAsync(Role role);
    Task<IdentityResult> DeleteAsync(Role role);
    Task<bool> HaveInDatabaseAsync(Expression<Func<Role, bool>> predicate);
    Task<IEnumerable<Role>> FindAllAsync(Expression<Func<Role, Role>>? selector = null);

    Task<Role?> FindByPredicateWithSelectorAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>>? selector = null,
        bool asNoTracking = false);
}