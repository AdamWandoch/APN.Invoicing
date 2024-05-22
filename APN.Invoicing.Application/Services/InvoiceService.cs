using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Exceptions;
using APN.Invoicing.Domain.Repositories;
using AutoMapper;
namespace APN.Invoicing.Application.Services;

public class InvoiceService(IUnitOfWork uow, IMapper mapper, IInvoiceRepository invoiceRepo, IOperationRepository operationRepo) : BaseService(uow, mapper), IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepo = invoiceRepo;
    private readonly IOperationRepository _operationRepo = operationRepo;

    public async Task<int> GenerateAsync(short month, short year, CancellationToken token)
    {
        _uow.BeginTransaction();
        try
        {
            if (!await _operationRepo.AreServiceStatusesValidAsync(month, year, token))
                throw new ServiceStatusInvalidException($"Service operations have invalid state in month {month} and year {year}.");

            if (await _operationRepo.IsCustomerWithInvoiceAndNotInvoicedOperationsAsync(month, year, token))
                throw new ServiceStatusInvalidException($"Exists a customer with an invoice and not invoiced operations in month {month} and year {year}.");

            // TODO: create invoices and invoice items based on available operations for the given period

            // 1. get operations for the period and group by customer

            // 2. create a list of invoices, one per customer

            //// 2.a for each invoice create a list of items based on operations

            // 3. save the invoices: await _invoiceRepository.Save(invoices, token)

            // 4. save the invoices items: await _invoiceRepository.SaveItems(invoiceItems, token)

            _uow.Commit();
            return 1;
        }
        catch (Exception)
        {
            _uow.Rollback();
            throw;
        }
    }

    public Task<InvoiceEntity> GetByCustomerMonthYearAsync(int customerID, short month, short year, CancellationToken token)
    {
        return _invoiceRepo.GetByCustomerIDMonthYearAsync(customerID, month, year, token);
    }
}
