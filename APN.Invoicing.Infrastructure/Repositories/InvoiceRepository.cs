using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Repositories;
using APN.Invoicing.Domain.UnitOfWork;
using Dapper;
using System.Text;

namespace APN.Invoicing.Infrastructure.Repositories;

public class InvoiceRepository(IUnitOfWork unitOfWork) : BaseRepository(unitOfWork), IInvoiceRepository
{
    public Task<IEnumerable<InvoiceEntity>> PersistInvoices(IEnumerable<OperationEntity> operations, short month, short year, CancellationToken token)
    {
        var sqlBuilder = new StringBuilder(
            @"INSERT INTO INVOICE (CustomerID, [Date], [Month], [Year])
            OUTPUT INSERTED.InvoiceID, INSERTED.CustomerID, INSERTED.[Date], INSERTED.[Month], INSERTED.[Year]
            VALUES ");

        var customerIDs = operations.Select(o => o.CustomerID).Distinct();

        foreach (var id in customerIDs)
        {
            sqlBuilder.Append('(');
            sqlBuilder.Append(id);
            sqlBuilder.Append(", SYSDATETIMEOFFSET(), ");
            sqlBuilder.Append(month);
            sqlBuilder.Append(',');
            sqlBuilder.Append(year);
            sqlBuilder.Append("),");
        }

        var sSql = sqlBuilder.Remove(sqlBuilder.Length - 1, 1).ToString();

        return _uow.Conn.QueryAsync<InvoiceEntity>(new CommandDefinition(sSql, transaction: _uow.Tx, cancellationToken: token));
    }

    public Task<int> PersistItems(IEnumerable<InvoiceItemEntity> invoiceItems, CancellationToken token)
    {
        var sSql = @"INSERT INTO INVOICE_ITEM (InvoiceID, ServiceID, StartOperationID, StopOperationID, StartDate, EndDate, 
                        StartMonth, EndMonth, StartYear, EndYear, [Value], Finished)
                    VALUES (@InvoiceID, @ServiceID, @StartOperationID, @StopOperationID, @StartDate, @EndDate, 
                        @StartMonth, @EndMonth, @StartYear, @EndYear, @Value, @Finished)";

        return _uow.Conn.ExecuteAsync(new CommandDefinition(sSql, invoiceItems, transaction: _uow.Tx, cancellationToken: token));
    }

    public async Task<InvoiceEntity?> GetByCustomerIDMonthYearAsync(int customerID, short month, short year, CancellationToken token)
    {
        var sSql = @"SELECT InvoiceID, CustomerID, [Date], [Month], [Year] 
                    FROM INVOICE
                    WHERE CustomerID = @customerID
                    	AND [Month] = @month
                    	AND [Year] = @year;
                    
                    SELECT InvItemID, II.InvoiceID, ServiceID, StartOperationID, StopOperationID, StartDate, EndDate, StartMonth, 
                        EndMonth, StartYear, EndYear, [Value], Finished
                    FROM INVOICE_ITEM II
                    JOIN INVOICE I ON I.InvoiceID = II.InvoiceID
                    	AND I.CustomerID = @customerID
                    	AND I.[Month] = @month
                    	AND I.[Year] = @year;";

        var param = new { customerID, month, year };

        var reader = await _uow.Conn.QueryMultipleAsync(new CommandDefinition(sSql, param, transaction: _uow.Tx, cancellationToken: token));

        var invoice = await reader.ReadFirstOrDefaultAsync<InvoiceEntity>();
        var invoiceItems = await reader.ReadAsync<InvoiceItemEntity>();

        if (invoice != null)
            return invoice with { Items = invoiceItems.ToList() };

        return null;
    }
}
