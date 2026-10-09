namespace OrderFlow.Application.Dtos;

public record CustomerDto(int Id, string Name, string Email, DateTime CreatedAtUtc);

// TODO (OF-108): add validation (required name, valid email, max lengths).
public record CreateCustomerRequest(string Name, string Email);
