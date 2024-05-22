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
        _logger.LogError(ex.Message);

        if (ex is ValidationBaseException)
        {
            return new ObjectResult(new { message = ex.Message })
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }

        return new ObjectResult(new { message = ex.Message, stackTrace = ex.StackTrace })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
