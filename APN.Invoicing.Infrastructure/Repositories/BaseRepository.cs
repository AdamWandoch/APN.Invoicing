namespace APN.Invoicing.Infrastructure.Repositories;

public class BaseRepository(IUnitOfWork unitOfWork)
{
    protected readonly IUnitOfWork _uow = unitOfWork;
}
