using APN.Invoicing.Domain.Entities;

namespace APN.Invoicing.Application.DTO
{
    public record OperationPostDTO
    {
        public int OperationID { get; init; }
        public int ServiceID { get; init; }
        public int CustomerID { get; init; }
        public decimal Quantity { get; init; }
        public decimal? PricePerDay { get; init; }
        public DateTimeOffset Date { get; init; } = DateTimeOffset.Now;
        public short Month => (short)Date.Month;
        public short Year => (short)Date.Year;
        public EnumOperationType Type { get; init; }
    }
}
