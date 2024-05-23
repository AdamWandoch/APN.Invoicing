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
        var lastOp = await _operationRepo.GetLastOperationByCustomerIDServiceIDAsync(operation.CustomerID, operation.ServiceID, operation.Month, operation.Year, token);

        if (lastOp == null && operation.Type == EnumOperationType.Start) return true;

        if (lastOp?.Date.UtcDateTime >= operation.Date.UtcDateTime) return false;

        return operation.Type switch
        {
            EnumOperationType.Start => lastOp?.Type == EnumOperationType.Stop,
            EnumOperationType.Pause => lastOp?.Type == EnumOperationType.Start || lastOp?.Type == EnumOperationType.Restart,
            EnumOperationType.Restart => lastOp?.Type == EnumOperationType.Pause,
            EnumOperationType.Stop => lastOp?.Type == EnumOperationType.Start || lastOp?.Type == EnumOperationType.Restart,
            _ => false,
        };
    }
}
