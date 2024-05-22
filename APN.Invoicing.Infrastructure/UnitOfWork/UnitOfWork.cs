using System.Data;

namespace APN.Invoicing.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _conn;
    private IDbTransaction _tx;

    public UnitOfWork(IDbConnection connection)
    {
        _conn = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public IDbConnection Conn => _conn;
    public IDbTransaction Tx => _tx;

    public void BeginTransaction()
    {
        if (_tx == null)
        {
            _tx = _conn.BeginTransaction();
        }
    }

    public void Commit()
    {
        try
        {
            _tx?.Commit();
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public void Rollback()
    {
        try
        {
            _tx?.Rollback();
        }
        finally
        {
            DisposeTransaction();
        }
    }

    private void DisposeTransaction()
    {
        _tx?.Dispose();
        _tx = null;
    }

    public void Dispose()
    {
        DisposeTransaction();
        _conn?.Dispose();
    }
}
