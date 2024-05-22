using APN.Invoicing.Application.DTO;
using APN.Invoicing.Application.Extensions;
using APN.Invoicing.Application.ServiceInterfaces;
using APN.Invoicing.Domain.Entities;
using APN.Invoicing.Domain.Exceptions;
using APN.Invoicing.Domain.Repositories;
using AutoMapper;
using FluentValidation;

namespace APN.Invoicing.Application.Services;

public class OperationService(IValidator<OperationPostDTO> operationValidator, IOperationRepository operationRepository, IUnitOfWork unitOfWork, IMapper mapper)
    : BaseService(unitOfWork, mapper), IOperationService
{
    private readonly IValidator<OperationPostDTO> _operationValidator = operationValidator;
    private readonly IOperationRepository _operationRepository = operationRepository;

    public async Task<int> PostAsync(OperationPostDTO dto, CancellationToken token)
    {
        _uow.BeginTransaction();
        try
        {
            var opDTOValidation = await _operationValidator.ValidateAsync(dto, token);
            if (!opDTOValidation.IsValid)
            {
                throw new OperationInvalidException(opDTOValidation.AsString());
            }

            var entity = _mapper.Map<OperationEntity>(dto);
            var result = await _operationRepository.SaveAsync(entity, token);
            _uow.Commit();
            return result;
        }
        catch
        {
            _uow.Rollback();
            throw;
        }
    }
}
