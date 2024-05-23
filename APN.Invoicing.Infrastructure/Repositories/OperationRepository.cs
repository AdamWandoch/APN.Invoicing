using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Repositories;
using APN.Invoicing.Domain.UnitOfWork;
using Dapper;

namespace APN.Invoicing.Infrastructure.Repositories;

public class OperationRepository(IUnitOfWork unitOfWork) : BaseRepository(unitOfWork), IOperationRepository
{
    public Task<int> SaveAsync(OperationEntity operation, CancellationToken token)
    {
        var sSql = @"INSERT INTO OPERATION 
                        (ServiceID, CustomerID,Quantity, PricePerDay, Date, Month, Year, Type)
                    VALUES (@ServiceID, @CustomerID, @Quantity, @PricePerDay, @Date, @Month, @Year, @Type)";

        return _uow.Conn.ExecuteAsync(new CommandDefinition(sSql, operation, transaction: _uow.Tx, cancellationToken: token));
    }

    public Task<OperationEntity?> GetLastOperationByCustomerIDServiceIDAsync(int customerID, int serviceID, short month, short year, CancellationToken token)
    {
        var sSql = @"SELECT TOP 1 OperationID, ServiceID, CustomerID, Quantity, PricePerDay, [Date], [Month], [Year], [Type] FROM OPERATION 
                WHERE CustomerID = @customerID AND ServiceID = @serviceID AND Month = @month AND Year = @year
                ORDER BY [Date] DESC, OperationID DESC";

        var param = new
        {
            customerID,
            serviceID,
            month,
            year
        };

        return _uow.Conn.QueryFirstOrDefaultAsync<OperationEntity>(new CommandDefinition(
            sSql, param, transaction: _uow.Tx, cancellationToken: token));
    }

    public async Task<bool> AreServiceStatusesValidAsync(short month, short year, CancellationToken token)
    {
        var sSql = @"WITH RankedOperations AS (
                        SELECT ServiceID, CustomerID, [Type],
                            ROW_NUMBER() OVER (PARTITION BY ServiceID, CustomerID ORDER BY [Date] DESC, OperationID DESC) rn
                        FROM OPERATION
                    	WHERE [Month] = @month and [Year] = @year
                    )
                    SELECT TOP 1 1
                    FROM RankedOperations
                    WHERE rn = 1 AND [Type] <> 4";

        var result = await _uow.Conn.QueryAsync<short>(new CommandDefinition(sSql, new { month, year },
            transaction: _uow.Tx, cancellationToken: token));

        return !result.Any();
    }

    public async Task<bool> IsCustomerWithInvoiceAndNotInvoicedOperationsAsync(short month, short year, CancellationToken token)
    {
        var sSql = @"SELECT TOP 1 1
                    FROM OPERATION O
                    LEFT JOIN INVOICE_ITEM II ON II.StopOperationID = O.OperationID
                    INNER JOIN INVOICE I ON I.CustomerID = O.CustomerID
                    WHERE O.[Type] = 4 
                        AND O.[Month] = @month 
                        AND O.[Year] = @year 
                        AND II.InvItemID IS NULL";

        var result = await _uow.Conn.QueryAsync<short>(new CommandDefinition(sSql, new { month, year },
            transaction: _uow.Tx, cancellationToken: token));

        return result.Any();
    }

    public Task<IEnumerable<OperationEntity>> GetOperationsForInvoicing(short month, short year, CancellationToken token)
    {
        var sSql = @"SELECT
                        O.OperationID, O.ServiceID, O.CustomerID, O.Quantity,
                        O.PricePerDay, O.[Date], O.[Month], O.[Year], O.[Type]
                    FROM OPERATION O
                    LEFT JOIN INVOICE_ITEM II ON O.OperationID = II.StartOperationID OR O.OperationID = II.StopOperationID
                    WHERE O.[Month] = @month 
                        AND O.[Year] = @year
                        AND II.InvItemID IS NULL
                    ORDER BY O.[Date] DESC, O.OperationID DESC";

        return _uow.Conn.QueryAsync<OperationEntity>(new CommandDefinition(sSql, new { month, year },
            transaction: _uow.Tx, cancellationToken: token));
    }
}
