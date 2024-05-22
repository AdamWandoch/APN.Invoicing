using APN.Invoicing.Application.DTO;
using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace APN.Invoicing.API.Controllers;

public class OperationsController(ILogger<OperationsController> logger, IOperationService operationService)
    : BaseController<OperationsController>(logger)
{
    private readonly IOperationService _operationService = operationService;

    [HttpPost]
    public async Task<ActionResult> Post(OperationPostDTO dto, CancellationToken token)
    {
        try
        {
            return Ok(await _operationService.PostAsync(dto, token));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
