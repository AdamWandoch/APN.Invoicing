using APN.Invoicing.Domain.Entities;

namespace APN.Invoicing.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<InvoiceEntity> GetByCustomerIDMonthYearAsync(int cutomerID, short month, short year, CancellationToken token);
    Task<IEnumerable<InvoiceEntity>> PersistInvoices(IEnumerable<OperationEntity> customerIDs, short month, short year, CancellationToken token);
    Task<int> PersistItems(IEnumerable<InvoiceItemEntity> invoiceItems, CancellationToken token);
}
