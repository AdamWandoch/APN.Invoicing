using APN.Invoicing.Application.DTO;
using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Repositories;
using APN.Invoicing.Domain.UnitOfWork;
using AutoMapper;

namespace APN.Invoicing.Application.Services;

public class OperationValidationService(IOperationRepository operationRepo, IUnitOfWork uow, IMapper mapper)
    : BaseService(uow, mapper), IOperationValidationService
{
    private readonly IOperationRepository _operationRepo = operationRepo;

    public async Task<bool> IsOperationAllowedAsync(OperationPostDTO operation, CancellationToken token)
    {
        var lastOpType = await _operationRepo.GetLastTypeByCustomerIDServiceIDAsync(operation.CustomerID, operation.ServiceID, operation.Month, operation.Year, token);

        if (lastOpType == 0 && operation.Type == EnumOperationType.Start) return true;

        return operation.Type switch
        {
            EnumOperationType.Start => lastOpType == EnumOperationType.Stop,
            EnumOperationType.Pause => lastOpType == EnumOperationType.Start || lastOpType == EnumOperationType.Restart,
            EnumOperationType.Restart => lastOpType == EnumOperationType.Pause,
            EnumOperationType.Stop => lastOpType == EnumOperationType.Start || lastOpType == EnumOperationType.Restart,
            _ => false,
        };
    }
}
