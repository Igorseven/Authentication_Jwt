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

    public async Task<bool> HaveInTheDatabaseAsync(Expression<Func<RefreshToken, bool>> where) => await _dbSetContext.AnyAsync(where);

    public async Task<bool> SaveAsync(RefreshToken refreshToken)
    {
        _dbSetContext.Add(refreshToken);

        return await SaveInDatabaseAsync();
    }

    public async Task<bool> DeleteAsync(string userName, string token)
    {
        var refreshToken = await _dbSetContext.FirstOrDefaultAsync(r => r.UserName == userName && r.Token == token);

        if (refreshToken is null) return false;

        DetachedObject(refreshToken);

        _dbSetContext.Remove(refreshToken);

        return await SaveInDatabaseAsync();
    }

    public async Task<bool> DeleteAsync(string userName)
    {
        var refreshToken = await _dbSetContext.FirstOrDefaultAsync(r => r.UserName == userName);

        if (refreshToken is null) return false;

        DetachedObject(refreshToken);

        _dbSetContext.Remove(refreshToken);

        return await SaveInDatabaseAsync();
    }
}

