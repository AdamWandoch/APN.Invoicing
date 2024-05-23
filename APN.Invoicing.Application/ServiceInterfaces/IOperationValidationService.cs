using APN.Invoicing.Application.DTO;

namespace APN.Invoicing.Application.ServiceInterfaces;

public interface IOperationValidationService
{
    Task<bool> IsOperationAllowedAsync(OperationPostDTO operation, CancellationToken token);
}
