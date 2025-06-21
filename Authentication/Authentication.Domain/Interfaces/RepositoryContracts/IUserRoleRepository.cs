using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Authentication.Domain.Interfaces.RepositoryContracts;

public interface IUserRoleRepository : IDisposable
{
    Task<bool> SaveAsync(UserRole userRole);
    Task<bool> DeleteAsync(UserRole userRole);

    Task<UserRole?> FindByPredicateAsync(
        Expression<Func<UserRole, bool>> predicate,
        bool asNoTracking = false);

    Task<IEnumerable<UserRole>> FindAllWithPredicateAsync(
        Expression<Func<UserRole, bool>> predicate,
        Func<IQueryable<UserRole>, IIncludableQueryable<UserRole, object>>? include = null);
}