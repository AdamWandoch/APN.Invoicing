using APN.Invoicing.Application.DTO;

namespace APN.Invoicing.Application.ServiceInterfaces;

public interface IOperationService
{
    /// <summary>
    /// Adds a customer operation
    /// </summary>
    /// <param name="dto">OperationPostDTO</param>
    /// <param name="token">Cancellation token</param>
    Task<int> PostAsync(OperationPostDTO dto, CancellationToken token);
}
