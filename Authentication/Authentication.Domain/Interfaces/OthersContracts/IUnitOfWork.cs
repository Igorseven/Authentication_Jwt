namespace Authentication.Domain.Interfaces.OthersContracts;
public interface IUnitOfWork
{
    void CommitTransaction();
    void RollbackTransaction();
    void BeginTransaction();
}
