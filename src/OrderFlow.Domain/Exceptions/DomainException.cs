namespace OrderFlow.Domain.Exceptions;

/// <summary>
/// Thrown when a business rule is violated (e.g. invalid status change, insufficient stock).
/// Mapped to HTTP 422 by the API's exception handler.
/// </summary>
public class DomainException(string message) : Exception(message);
