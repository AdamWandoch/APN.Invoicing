using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Repositories;
using Dapper;

namespace APN.Invoicing.Infrastructure.Repositories;

public class InvoiceRepository(IUnitOfWork unitOfWork) : BaseRepository(unitOfWork), IInvoiceRepository
{
    public Task<InvoiceEntity> GetByCustomerIDMonthYearAsync(int customerID, short month, short year, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveAsync(InvoiceEntity entity, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveListAsync(IEnumerable<InvoiceEntity> entity, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
