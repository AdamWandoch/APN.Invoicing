using APN.Invoicing.Domain.Entities;

namespace APN.Invoicing.Domain.Repositories;

public interface IOperationRepository
{
    Task<int> SaveAsync(OperationEntity operation, CancellationToken token);
    Task<EnumOperationType> GetLastTypeByCustomerIDServiceIDAsync(int customerID, int serviceID, short month, short year, CancellationToken token);
    Task<bool> AreServiceStatusesValidAsync(short month, short year, CancellationToken token);
    Task<bool> IsCustomerWithInvoiceAndNotInvoicedOperationsAsync(short month, short year, CancellationToken token);
}
