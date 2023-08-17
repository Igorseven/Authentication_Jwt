using Authentication.Infrastructure.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Repository.Base;
public abstract class BaseRepository<T> : IDisposable where T : class
{
    protected readonly ApplicationContext _context;
    protected DbSet<T> _dbSetContext => _context.Set<T>();

    public BaseRepository(ApplicationContext context)
    {
        _context = context;
    }
    public void Dispose() => _context.Dispose();

    protected async Task<bool> SaveInDatabaseAsync() => await _context.SaveChangesAsync() > 0;

    protected void DetachedObject(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSetContext.Attach(entity);
    }

}
