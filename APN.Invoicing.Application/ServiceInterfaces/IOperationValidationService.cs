using APN.Invoicing.Application.DTO;
//using APN.Invoicing.Domain.Entities;

namespace APN.Invoicing.Application.ServiceInterfaces;

public interface IOperationValidationService
{
    Task<bool> IsOperationAllowedAsync(OperationPostDTO operation, CancellationToken token);
    //Task<EnumOperationType> GetLastTypeByCustomerIDServiceIDAsync(int customerID, int serviceID, short month, short year, CancellationToken token);
}
