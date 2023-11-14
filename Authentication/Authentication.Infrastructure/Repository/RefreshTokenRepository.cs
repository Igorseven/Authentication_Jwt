using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repository;
public sealed class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<bool> HaveInTheDatabaseAsync(Expression<Func<RefreshToken, bool>> where) => await DbSetContext.AnyAsync(where);

    public async Task<bool> SaveAsync(RefreshToken refreshToken)
    {
        DbSetContext.Add(refreshToken);

        return await SaveInDatabaseAsync();
    }

    public async Task<bool> DeleteAsync(string userName, string token)
    {
        var refreshToken = await DbSetContext.FirstOrDefaultAsync(r => r.UserName == userName && r.Token == token);

        if (refreshToken is null) return false;

        DetachedObject(refreshToken);

        DbSetContext.Remove(refreshToken);

        return await SaveInDatabaseAsync();
    }

    public async Task<bool> DeleteAsync(string userName)
    {
        var refreshToken = await DbSetContext.FirstOrDefaultAsync(r => r.UserName == userName);

        if (refreshToken is null) return false;

        DetachedObject(refreshToken);

        DbSetContext.Remove(refreshToken);

        return await SaveInDatabaseAsync();
    }
}

