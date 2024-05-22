using System.Data;

namespace APN.Invoicing.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IDbConnection Conn { get; }
    IDbTransaction Tx { get; }
    void BeginTransaction();
    void Commit();
    void Rollback();
}