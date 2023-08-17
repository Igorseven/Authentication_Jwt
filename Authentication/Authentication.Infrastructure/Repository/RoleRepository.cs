using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repository;
public sealed class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    private readonly RoleManager<Role> _roleManager;
    public RoleRepository(ApplicationContext context, RoleManager<Role> roleManager)

        : base(context)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> HaveInDatabaseAsync(Expression<Func<Role, bool>> predicate) => await _dbSetContext.AnyAsync(predicate);

    public async Task<IEnumerable<Role>> FindAllAsync(Expression<Func<Role, Role>>? selector = null)
    {
        IQueryable<Role> query = _dbSetContext;

        if (selector is not null)
            query = query.Select(selector);

        query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<Role?> FindByPredicateWithSelectorAsync(Expression<Func<Role, bool>> predicate, Expression<Func<Role, Role>>? selector = null, bool asNoTracking = false)
    {
        IQueryable<Role> query = _dbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        if (selector is not null)
            query = query.Select(selector);

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<Role?> FindByPredicateAsync(Expression<Func<Role, bool>> predicate, bool asNoTracking = false)
    {
        IQueryable<Role> query = _dbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IdentityResult> SaveAsync(Role role) => await _roleManager.CreateAsync(role);


    public async Task<IdentityResult> DeleteAsync(Role role)
    {
        DetachedObject(role);

        return await _roleManager.DeleteAsync(role);
    }
}

