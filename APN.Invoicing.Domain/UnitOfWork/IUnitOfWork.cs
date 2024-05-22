using System.Data;

public interface IUnitOfWork : IDisposable
{
    IDbConnection Conn { get; }
    IDbTransaction Tx { get; }
    void BeginTransaction();
    void Commit();
    void Rollback();
}
