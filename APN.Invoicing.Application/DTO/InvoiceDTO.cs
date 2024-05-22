namespace APN.Invoicing.Application.DTO;

public record InvoiceDTO(
    int InvoiceID,
    int CustomerID,
    DateTimeOffset Date,
    List<InvoiceItemDTO> Items);

public record InvoiceItemDTO(
    int InvItemID,
    int InvoiceID,
    int ServiceID,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    decimal Value,
    bool Finished);
