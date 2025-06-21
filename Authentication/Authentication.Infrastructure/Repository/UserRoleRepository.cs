using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repository;
public sealed class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserRole>> FindAllWithPredicateAsync(
        Expression<Func<UserRole, bool>> predicate, 
        Func<IQueryable<UserRole>, IIncludableQueryable<UserRole, object>>? include = null)
    {
        IQueryable<UserRole> query = DbSetContext;

        if (include is not null)
            query = include(query);

        query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<UserRole?> FindByPredicateAsync(
        Expression<Func<UserRole, bool>> predicate, 
        bool asNoTracking = false)
    {
        IQueryable<UserRole> query = DbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> SaveAsync(UserRole userRole)
    {
        await DbSetContext.AddAsync(userRole);

        return await SaveInDatabaseAsync();
    }

    public async Task<bool> DeleteAsync(UserRole userRole)
    {
        DetachedObject(userRole);

        DbSetContext.Remove(userRole);

        return await SaveInDatabaseAsync();
    }
}

