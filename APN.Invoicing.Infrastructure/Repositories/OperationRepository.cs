using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Repositories;
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

    public Task<EnumOperationType> GetLastTypeByCustomerIDServiceIDAsync(int customerID, int serviceID, short month, short year, CancellationToken token)
    {
        var sSql = @"SELECT TOP 1 Type FROM OPERATION 
                WHERE CustomerID = @customerID AND ServiceID = @serviceID AND Month = @month AND Year = @year
                ORDER BY [Date] DESC, OperationID DESC";

        var param = new
        {
            customerID,
            serviceID,
            month,
            year
        };

        return _uow.Conn.QueryFirstOrDefaultAsync<EnumOperationType>(new CommandDefinition(
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
}
