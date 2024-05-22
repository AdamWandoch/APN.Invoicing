using APN.Invoicing.Application.DTO;
using APN.Invoicing.Domain.Entities;
using AutoMapper;

namespace APN.Invoicing.Application.Mapping;

public class OperationProfile : Profile
{
    public OperationProfile()
    {
        CreateMap<OperationPostDTO, OperationEntity>()
                .ConstructUsing(dto => new OperationEntity(
                    dto.OperationID,
                    dto.ServiceID,
                    dto.CustomerID,
                    dto.Quantity,
                    dto.PricePerDay,
                    dto.Date,
                    dto.Month,
                    dto.Year,
                    dto.Type
                ));
    }
}
