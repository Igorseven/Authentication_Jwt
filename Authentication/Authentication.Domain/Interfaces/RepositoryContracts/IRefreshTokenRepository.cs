using Authentication.Domain.Entities;
using System.Linq.Expressions;

namespace Authentication.Domain.Interfaces.RepositoryContracts;
public interface IRefreshTokenRepository : IDisposable
{
    Task<bool> SaveAsync(RefreshToken refreshToken);
    Task<bool> DeleteAsync(string userName, string refreshToken);
    Task<bool> DeleteAsync(string userName);
    Task<bool> HaveInTheDatabaseAsync(Expression<Func<RefreshToken, bool>> where);
}
