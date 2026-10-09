using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Dtos;
using OrderFlow.Application.Services;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(ICustomerService customerService, IOrderService orderService) : ControllerBase
{
    /// <summary>GET /api/customers</summary>
    [HttpGet]
    public ActionResult<List<CustomerDto>> GetAll() => Ok(customerService.GetAll());

    /// <summary>GET /api/customers/{id}</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken ct) =>
        Ok(await customerService.GetByIdAsync(id, ct));

    /// <summary>POST /api/customers</summary>
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerRequest request, CancellationToken ct)
    {
        var created = await customerService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>GET /api/customers/{id}/orders</summary>
    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<List<OrderDto>>> GetOrders(int id, CancellationToken ct) =>
        Ok(await orderService.GetForCustomerAsync(id, ct));
}
