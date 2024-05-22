namespace APN.Invoicing.Domain.Exceptions;

public class ExistingInvoiceAndNotInvoicedOperations(string message) : ValidationBaseException(message) { }
