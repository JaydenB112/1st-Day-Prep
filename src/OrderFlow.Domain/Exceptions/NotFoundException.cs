namespace OrderFlow.Domain.Exceptions;

/// <summary>
/// Thrown when a requested entity does not exist. Mapped to HTTP 404 by the API's exception handler.
/// </summary>
public class NotFoundException(string entityName, object key)
    : Exception($"{entityName} with id '{key}' was not found.");
