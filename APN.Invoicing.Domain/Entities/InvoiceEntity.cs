namespace APN.Invoicing.Domain.Entities;

public record InvoiceEntity(
    int InvoiceID,
    int CustomerID,
    DateTimeOffset Date,
    short Month,
    short Year,
    List<InvoiceItemEntity> Items);

public record InvoiceItemEntity(
    int InvItemID,
    int InvoiceID,
    int ServiceID,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    short StartMonth,
    short EndMonth,
    short StartYear,
    short EndYear,
    decimal Value,
    bool Finished);
