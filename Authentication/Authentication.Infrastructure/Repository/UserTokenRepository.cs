using Authentication.Domain.Entities;
using Authentication.Domain.Interfaces.RepositoryContracts;
using Authentication.Infrastructure.ORM.Context;
using Authentication.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Infrastructure.Repository;
public sealed class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
{
    private const int NumberChangesInDatabase = 1;
    
    public UserTokenRepository(ApplicationContext context) : base(context)
    {
    }

    public Task<bool> HaveInTheDatabaseAsync(Expression<Func<UserToken, bool>> where) => 
        DbSetContext.AnyAsync(where);

    public async Task<bool> SaveAsync(UserToken refreshToken)
    {
        await DbSetContext.AddAsync(refreshToken);

        return await SaveInDatabaseAsync();
    }
    
    public async Task<bool> DeleteAsync(Expression<Func<UserToken, bool>> predicate) =>
        await DbSetContext.Where(predicate).ExecuteDeleteAsync() > NumberChangesInDatabase;
}

