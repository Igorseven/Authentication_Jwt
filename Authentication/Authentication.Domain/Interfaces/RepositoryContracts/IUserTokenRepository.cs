using Authentication.Domain.Entities;
using System.Linq.Expressions;

namespace Authentication.Domain.Interfaces.RepositoryContracts;
public interface IUserTokenRepository : IDisposable
{
    Task<bool> SaveAsync(UserToken refreshToken);
    Task<bool> DeleteAsync(Expression<Func<UserToken, bool>> predicate);
    
    Task<bool> HaveInTheDatabaseAsync(Expression<Func<UserToken, bool>> where);
}
