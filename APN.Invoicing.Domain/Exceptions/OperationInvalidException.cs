namespace APN.Invoicing.Domain.Exceptions;

public class OperationInvalidException(string message) : ValidationBaseException(message) { }
