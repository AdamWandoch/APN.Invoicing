using APN.Invoicing.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace APN.Invoicing.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController<T>(ILogger<T> logger) : ControllerBase
{
    protected readonly ILogger<T> _logger = logger;

    protected ObjectResult HandleException(Exception ex)
    {

        if (ex is OperationsForInvoicingNotFoundException or CustomerDataNotFoundException)
        {
            _logger.LogInformation(ex.Message);
            return ObjectResult(ex, StatusCodes.Status409Conflict);
        }

        if (ex is ValidationBaseException)
        {
            _logger.LogWarning(ex.Message);
            return ObjectResult(ex, StatusCodes.Status422UnprocessableEntity);
        }

        _logger.LogError(ex.Message);
        return ObjectResult(ex, StatusCodes.Status500InternalServerError);
    }

    private static ObjectResult ObjectResult(Exception ex, int statusCode)
    {
        var showStackTrace =
            ex is not OperationsForInvoicingNotFoundException &&
            ex is not CustomerDataNotFoundException &&
            ex is not ValidationBaseException;

        dynamic data = showStackTrace
            ? new { message = ex.Message, stackTrace = ex.StackTrace }
            : new { message = ex.Message };

        return new ObjectResult(data)
        {
            StatusCode = statusCode
        };
    }
}
