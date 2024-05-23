using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Exceptions;
using APN.Invoicing.Domain.Repositories;
using APN.Invoicing.Domain.UnitOfWork;
using AutoMapper;
using System.Globalization;

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

            var operations = await _operationRepo.GetOperationsForInvoicing(month, year, token);
            if (!operations.Any())
                throw new OperationsForInvoicingNotFoundException($"No operations available for invoicing in month {month} and year {year}.");

            var persistedInvoices = await _invoiceRepo.PersistInvoices(operations, month, year, token);

            var invoiceItems = GenerateItemObjects(persistedInvoices, operations);
            var result = await _invoiceRepo.PersistItems(invoiceItems, token);

            _uow.Commit();
            return result;
        }
        catch (Exception)
        {
            _uow.Rollback();
            throw;
        }
    }

    private static List<InvoiceItemEntity> GenerateItemObjects(IEnumerable<InvoiceEntity> createdInvoices, IEnumerable<OperationEntity> operations)
    {
        var invoiceItems = new List<InvoiceItemEntity>();

        foreach (var invoice in createdInvoices)
        {
            var customerOps = operations.Where(o => o.CustomerID == invoice.CustomerID).OrderBy(o => o.Date).ToList();
            var serviceIDs = customerOps.Select(op => op.ServiceID).Distinct();
            foreach (var serviceID in serviceIDs)
            {
                var opsPerService = customerOps.Where(op => op.ServiceID == serviceID).ToList();
                while (opsPerService.Count != 0)
                {
                    #region [ find continuous period of service provisioned ]

                    var startOp = opsPerService.First(o => o.ServiceID == serviceID
                        && (o.Type == EnumOperationType.Start || o.Type == EnumOperationType.Restart));
                    opsPerService.Remove(startOp);

                    var stopOp = opsPerService.First(o => o.ServiceID == serviceID
                        && (o.Type == EnumOperationType.Pause || o.Type == EnumOperationType.Stop));
                    opsPerService.Remove(stopOp);

                    #endregion 

                    #region [ calculate item value based on duration in days in decimal value respecting decimal places of a double ]

                    decimal durationDays = (decimal)(stopOp.Date - startOp.Date).TotalDays;
                    int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(durationDays)[3])[2];
                    string format = "F" + decimalPlaces.ToString(CultureInfo.InvariantCulture);
                    string formattedDecimal = durationDays.ToString(format, CultureInfo.InvariantCulture);
                    durationDays = decimal.Parse(formattedDecimal, CultureInfo.InvariantCulture);

                    var value = (durationDays * startOp.PricePerDay * startOp.Quantity) ?? 0m;

                    #endregion

                    invoiceItems.Add(new InvoiceItemEntity(
                        0, invoice.InvoiceID, serviceID, startOp.OperationID, stopOp.OperationID,
                        startOp.Date, stopOp.Date, startOp.Month, stopOp.Month, startOp.Year, stopOp.Year,
                        value, stopOp.Type == EnumOperationType.Pause || stopOp.Type == EnumOperationType.Stop));
                }
            }
        }

        return invoiceItems;
    }

    public Task<InvoiceEntity> GetByCustomerMonthYearAsync(int customerID, short month, short year, CancellationToken token)
    {
        return _invoiceRepo.GetByCustomerIDMonthYearAsync(customerID, month, year, token);
    }
}
