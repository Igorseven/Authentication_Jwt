using Authentication.Domain.Interfaces.OthersContracts;
using Authentication.Infrastructure.ORM.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Authentication.Infrastructure.ORM.UoW;
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseFacade _databaseFacade;

    public UnitOfWork(ApplicationContext context)
    {
        _databaseFacade = context.Database;
    }

    public void CommitTransaction()
    {
        try
        {
            _databaseFacade.CommitTransaction();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
    }

    public void RollbackTransaction() => _databaseFacade.RollbackTransaction();

    public void BeginTransaction() => _databaseFacade.BeginTransaction();
}
