using APN.Invoicing.API.Validation;
using APN.Invoicing.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace APN.Invoicing.Controllers
{
    public class InvoicesController(ILogger<InvoicesController> logger, IInvoiceService invoiceService) : BaseController<InvoicesController>(logger)
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        [HttpPost("calculate")]
        public async Task<ActionResult> Calculate([FromQuery] CalculateInvoiceParams p, CancellationToken token)
        {
            try
            {
                return Ok(await _invoiceService.GenerateAsync(p.Month, p.Year, token));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetInvoiceParams p, CancellationToken token)
        {
            try
            {
                return Ok(await _invoiceService.GetByCustomerMonthYearAsync(p.CustomerID, p.Month, p.Year, token));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
