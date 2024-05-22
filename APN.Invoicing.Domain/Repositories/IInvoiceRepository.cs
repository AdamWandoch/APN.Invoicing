using APN.Invoicing.Domain.Entities;

namespace APN.Invoicing.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<int> SaveAsync(InvoiceEntity entity, CancellationToken token);
    Task<int> SaveListAsync(IEnumerable<InvoiceEntity> entity, CancellationToken token);
    Task<InvoiceEntity> GetByCustomerIDMonthYearAsync(int cutomerID, short month, short year, CancellationToken token);
}
