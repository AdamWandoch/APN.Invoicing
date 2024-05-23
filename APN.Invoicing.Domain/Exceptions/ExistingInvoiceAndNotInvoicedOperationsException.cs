namespace APN.Invoicing.Domain.Exceptions;

public class ExistingInvoiceAndNotInvoicedOperationsException(string message) : ValidationBaseException(message) { }
