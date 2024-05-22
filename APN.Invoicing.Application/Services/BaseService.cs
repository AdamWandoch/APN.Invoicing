using AutoMapper;

namespace APN.Invoicing.Application.Services;

public class BaseService(IUnitOfWork uow, IMapper mapper)
{
    protected readonly IUnitOfWork _uow = uow;
    protected readonly IMapper _mapper = mapper;
}
