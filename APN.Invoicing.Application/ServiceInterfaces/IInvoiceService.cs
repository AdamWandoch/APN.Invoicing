using APN.Invoicing.Domain.Entities;

namespace APN.Invoicing.Application.ServiceInterfaces;

public interface IInvoiceService
{
    /// <summary>
    /// Generates invoices for the given period
    /// </summary>
    /// <param name="month">Month</param>
    /// <param name="year">Year</param>
    /// <param name="token">Cancellation token</param>
    Task<int> GenerateAsync(short month, short year, CancellationToken token);

    /// <summary>
    /// Returns the customer invoice for the given period
    /// </summary>
    /// <param name="cutomerID">CustomerID</param>
    /// <param name="month">Month</param>
    /// <param name="year">Year</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>Customer invoice</returns>
    Task<InvoiceEntity> GetByCustomerMonthYearAsync(int cutomerID, short month, short year, CancellationToken token);
}
