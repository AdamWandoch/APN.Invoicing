namespace APN.Invoicing.Domain.Exceptions;

public class ServiceStatusInvalidException(string message) : ValidationBaseException(message) { }
