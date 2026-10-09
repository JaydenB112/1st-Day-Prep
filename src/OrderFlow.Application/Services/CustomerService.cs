using OrderFlow.Application.Dtos;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Application.Services;

public interface ICustomerService
{
    List<CustomerDto> GetAll();
    Task<CustomerDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);
}

public class CustomerService(ICustomerRepository customers, IUnitOfWork unitOfWork) : ICustomerService
{
    public List<CustomerDto> GetAll()
    {
        var all = customers.GetAllAsync().Result;
        return all.Select(ToDto).ToList();
    }

    public async Task<CustomerDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var customer = await customers.GetByIdAsync(id, ct);
        return ToDto(customer!);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
    {
        var customer = new Customer { Name = request.Name, Email = request.Email };
        await customers.AddAsync(customer, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return ToDto(customer);
    }

    private static CustomerDto ToDto(Customer c) => new(c.Id, c.Name, c.Email, c.CreatedAtUtc);
}
