using APN.Invoicing.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace APN.Invoicing.Controllers
{
    public class InvoicesController(ILogger<InvoicesController> logger, IInvoiceService invoiceService) : BaseController<InvoicesController>(logger)
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        [HttpPost("calculate")]
        public async Task<ActionResult> Calculate(short month, short year, CancellationToken token)
        {
            try
            {
                return Ok(await _invoiceService.GenerateAsync(month, year, token));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get(short customerID, short month, short year, CancellationToken token)
        {
            try
            {
                return Ok(await _invoiceService.GetByCustomerMonthYearAsync(customerID, month, year, token));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
