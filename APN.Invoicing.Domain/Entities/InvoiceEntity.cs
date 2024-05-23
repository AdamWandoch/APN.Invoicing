namespace APN.Invoicing.Domain.Entities;

public record InvoiceEntity(
    int InvoiceID,
    int CustomerID,
    DateTimeOffset Date,
    short Month,
    short Year,
    List<InvoiceItemEntity> Items)
{
    /// <summary>
    /// Default, parameterless constructor for object materialization
    /// </summary>
    public InvoiceEntity() : this(default, default, default, default, default, []) { }
};



public record InvoiceItemEntity(
    int InvItemID,
    int InvoiceID,
    int ServiceID,
    int StartOperationID,
    int StopOperationID,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    short StartMonth,
    short EndMonth,
    short StartYear,
    short EndYear,
    decimal Value,
    bool Finished)
{
    /// <summary>
    /// Default, parameterless constructor for object materialization
    /// </summary>
    public InvoiceItemEntity() : this(default, default, default, default, default, default,
                             default, default, default, default, default, default, default)
    { }
};

