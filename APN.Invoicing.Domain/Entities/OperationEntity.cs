namespace APN.Invoicing.Domain.Entities;

public record OperationEntity(
    int OperationID,
    int ServiceID,
    int CustomerID,
    decimal Quantity,
    decimal? PricePerDay,
    DateTimeOffset Date,
    short Month,
    short Year,
    EnumOperationType Type);

public enum EnumOperationType : short
{
    Start = 1,
    Pause,
    Restart,
    Stop
}
